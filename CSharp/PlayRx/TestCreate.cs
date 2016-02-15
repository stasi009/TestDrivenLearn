using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

namespace PlayRx
{
    /// <summary>
    /// Observable.Create. since C# doesn't support anonymous interface implementation like Java, 
    /// this Create method is just a substitute, the body given in the Create is just 
    /// the operation expected when "IObservable.Subscribe" is invoked. 
    /// note: more sound like "Observable.Create" is for Cold Observable, because it is totally state-less
    /// </summary>
    sealed class TestCreate
    {
        private static void SampleOfCreate()
        {
            IEnumerable<int> sequence = null;
            TimeSpan interval = TimeSpan.FromDays(1);

            IObservable<int> source = Observable.Create<int>(
                observer =>
                {
                    foreach (int num in sequence)
                    {
                        observer.OnNext(num);
                        Thread.Sleep(interval);
                    }
                    observer.OnCompleted();

                    return () => Console.WriteLine("subscription disposed");
                });

            // ******************** change that captured variables
            sequence = Enumerable.Range(10, 3);
            interval = TimeSpan.FromSeconds(1);

            IDisposable subscription = source.Subscribe(new ConsolePrintObserver<int>());
            subscription.Dispose();
        }

        private static void SampleOfCreate_SubscribeOn(IScheduler subScheduler, string hint)
        {
            IObservable<string> source = Helper.MakeConsoleInputObservable();

            IObservable<string> toSubscribe = subScheduler == null ? source : source.SubscribeOn(subScheduler);
            toSubscribe.Subscribe(
                str => Console.WriteLine("\t" + str),
                () => Console.WriteLine("\t!!! data source completed !!!"));

            Console.WriteLine(hint);

            new ManualResetEvent(false).WaitOne();
        }

        // Create and ReplaySubject can serve the same purpose, which is used to create an observable
        // but the difference is that, ReplaySubject is stateful, you can create ReplaySubject once
        // and subscribed multiple times, during those multiple subscription, the data generation won't repeat
        // but "Create" returns a cold observable, everytime subscribed, it will repeat the data generation
        // with the newest data
        #region "Create vs. ReplaySubject"

        abstract class GeneratorBase
        {
            private int m_numInvoke = 0;
            public int NumInvoke { get { return m_numInvoke; } }

            private int m_seed = 0;
            public int Seed { set { m_seed = value; } }

            public abstract IObservable<int> Generate();

            protected void Fill(IObserver<int> sink)
            {
                ++m_numInvoke;

                foreach (int num in Enumerable.Range(1, 3))
                {
                    sink.OnNext(m_seed + num);
                }
                sink.OnCompleted();

                Console.WriteLine("****** data loaded.");
            }
        }

        sealed class Generator_ReplaySubject : GeneratorBase
        {
            public override IObservable<int> Generate()
            {
                ISubject<int> subject = new ReplaySubject<int>();
                Fill(subject);
                return subject;
            }
        }

        sealed class Generator_Create : GeneratorBase
        {
            public override IObservable<int> Generate()
            {
                return Observable.Create<int>(observer =>
                                             {
                                                 Fill(observer);
                                                 return Disposable.Empty;
                                             });
            }
        }

        sealed class Sumer
        {
            private int m_numSubscription = 0;
            public int NumSubscription { get { return m_numSubscription; } }

            public int SyncSum(IObservable<int> source)
            {
                ++m_numSubscription;

                int total = 0;
                source.Subscribe(num => total += num);

                return total;
            }
        }

        /// <summary>
        /// the IObservable generated with ReplaySubject can be generated once and used for multiple times
        /// </summary>
        private static void TestGenerate_ReplaySubject_ReuseFeature()
        {
            GeneratorBase generator = new Generator_ReplaySubject();
            IObservable<int> reusableSource = generator.Generate();

            Sumer sumer = new Sumer();
            int total = sumer.SyncSum(reusableSource);
            Debug.Assert(total == 6);

            // ---------------------- change the seed won't affect generated observables
            // ---------------------- because this observable is a concrete sequence, not just a factory
            generator.Seed = 100;
            total = sumer.SyncSum(reusableSource);
            Debug.Assert(total == 6);// it WON'T generate again with the latest value

            Debug.Assert(1 == generator.NumInvoke);

            Console.WriteLine("consume [{0}] times,data generated [{1}] times", sumer.NumSubscription, generator.NumInvoke);
        }

        /// <summary>
        /// chekanote: this test demonstrate that the observable returned by "Create" is a cold one
        /// 1. "Create" won't run until there is observer subscribed on
        /// 2. everytime an observer subscribed, "Create" will be invoked once again
        /// 3. everytime an observer subscribed, "Create" will generate and publish newest value with latest seed 
        /// </summary>
        private static void TestGenerate_Create_ColdFeature()
        {
            GeneratorBase generator = new Generator_Create();
            IObservable<int> coldSource = generator.Generate();

            Sumer sumer = new Sumer();
            int total = sumer.SyncSum(coldSource);
            Debug.Assert(total == 6);

            total = sumer.SyncSum(coldSource);
            Debug.Assert(total == 6);
            Debug.Assert(2 == generator.NumInvoke);

            // ---------------------- change the seed will affect generated observables
            // ---------------------- because this observable ISN'T a concrete sequence, just a factory
            // ---------------------- everytime subscribed, it will use the latest seed to generate and publish data
            generator.Seed = 100;
            total = sumer.SyncSum(coldSource);
            Debug.Assert(306 == total);
            Debug.Assert(3 == generator.NumInvoke);

            Console.WriteLine("consume [{0}] times,data generated [{1}] times", sumer.NumSubscription, generator.NumInvoke);
        }

        #endregion

        /// <summary>
        /// note: the Observable generated by "Create" can automatically unsubscribe its observers when completed
        /// </summary>
        [Test]
        public static void TestAutoUnsubscribe()
        {
            bool isDisposed = false;

            IObservable<int> source = Observable.Create<int>(observer =>
            {
                observer.OnNext(1);
                observer.OnCompleted();// chekanote: without calling "OnCompleted", it won't auto-unsubscribe
                return () => { isDisposed = true; };
            });

            source.Subscribe(_ => { });

            Assert.IsTrue(isDisposed);

        }

        /// <summary>
        /// note: as long as there is observer subscribed on, the function within "Create" begin execution until end
        /// it won't care how many data its observer want to consume, it will generate all the data
        /// </summary>
        private static void TestNonStopProducer()
        {
            const int TotalLoop = 5;
            const int ExpectedNum = 3;

            IObservable<int> nonStopSource = Observable.Create<int>(observer =>
            {
                for (int index = 0; index < TotalLoop; index++)
                {
                    Console.WriteLine("producing <{0}>, ......", index);
                    Thread.Sleep(TimeSpan.FromSeconds(0.5));
                    observer.OnNext(index);
                }
                observer.OnCompleted();
                Console.WriteLine("!!! production is completed.");

                return Disposable.Empty;
            });

            nonStopSource.Take(ExpectedNum).Subscribe(
                num => Console.WriteLine("\t<{0}> consumed.", num),
                () => Console.WriteLine("\t!!! consumption is completed."));
        }

        /// <summary>
        /// chekanote: one way to make an early-terminable producer is letting the data production and return that IDisposable
        /// run in parallel. if they are in sequence (such as, only return the IDispoable after the production is 
        /// totally completed, there won't be any chance to cancel the production)
        /// </summary>
        private static void TestEarlyTerminateProducer()
        {
            const int TotalLoop = 5;
            const int ExpectedNum = 3;

            IObservable<int> stopableSource = Observable.Create<int>(observer =>
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken cancelToken = cts.Token;

                Task producerTask = Task.Factory.StartNew(() =>
                {
                    for (int index = 0; index < TotalLoop; index++)
                    {
                        cancelToken.ThrowIfCancellationRequested();

                        Console.WriteLine("producing <{0}>, ......", index);
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        observer.OnNext(index);
                    }

                    observer.OnCompleted();
                    Console.WriteLine("### production is fully completed.");
                }, cancelToken);

                return () =>
                           {
                               cts.Cancel();
                               Console.WriteLine("!!! production is cancelled.");
                               observer.OnCompleted();
                           };
            });

            stopableSource.Take(ExpectedNum).Subscribe(num => Console.WriteLine("\t<{0}> consumed.", num));

            Helper.Pause();
        }

        public static void TestMain()
        {
            // SampleOfCreate();
            // SampleOfCreate_SubscribeOn(null, "subscribed, and also, source completed");

            // note: for cold observable, by specifying the thread on which the subscription is made
            // the thread on which data is generated is also specified to that same subscription thread
            // so here data source and target run on different thread from the main thread
            SampleOfCreate_SubscribeOn(Scheduler.NewThread, "subscribed, but source is still running, ......");

            // TestGenerate_ReplaySubject_ReuseFeature();
            // TestGenerate_Create_ColdFeature();

            // TestNonStopProducer();
            // TestEarlyTerminateProducer();
        }
    }
}
