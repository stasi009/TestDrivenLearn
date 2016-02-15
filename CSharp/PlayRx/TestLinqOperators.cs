using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;
using System.Reactive.Linq;

using NUnit.Framework;

namespace PlayRx
{
    [TestFixture]
    sealed class TestLinqOperators
    {
        private static void TestFirst_SyncSource()
        {
            int firstNum = Helper.DelayBlockEnumerable(1, 3, TimeSpan.FromSeconds(0.1)).ToObservable()
                .Do(num => Console.WriteLine("processing {0}, ......", num))
                .First();
            Console.WriteLine("after iterating the whole list, we get first number=<{0}>", firstNum);

            // chekanote: since 'MakeConsoleInputObservable' is an infinite loop which will block the main thread
            // so unless you input "exit" and return from the loop, 
            // otherwise you cannot see that the first string 
            string firstString = Helper.MakeConsoleInputObservable().First();
            Console.WriteLine("first console input: '{0}'", firstString);
        }

        private static void TestFirst_AsyncSource()
        {
            // chekanote: the production will stop as soon as the first qualified number is generated
            long firstNum = Observable.Interval(TimeSpan.FromSeconds(0.1))
                .Do(num => Console.WriteLine("processing {0}, ......", num))
                .First(num => num > 3);
            Console.WriteLine("first qualifed number found: {0}", firstNum);

            // chekanote: below codes "SubscribeOn" is very important, it subscribe the source on a new thread
            // since that source is a cold observable, so it will start the observable on a different thread
            // which will not block the main thread. as long as you input the first string, the main thread 
            // can see that result. you don't need to input "exit" to end that loop
            string firstString = Helper.MakeConsoleInputObservable().SubscribeOn(Scheduler.NewThread).First();
            Console.WriteLine("first console input: '{0}'", firstString);
        }

        private static void TestFirst_Empty()
        {
            IObservable<int> emptySource = Observable.Empty<int>();

            Assert.Throws<InvalidOperationException>(() => emptySource.First());

            int defvalue = emptySource.FirstOrDefault();
            Assert.AreEqual(0, defvalue);

            Console.WriteLine("test succeeds");
        }

        private static void TestAny()
        {
            var proxy = new int[] { 1, 3, 4, 5, 7 }.ToObservableWithInterval(TimeSpan.FromSeconds(1)).Publish();

            proxy.Subscribe(
                num => Console.WriteLine("checking {0},......", num),
                () => Console.WriteLine("original source completed"));

            // the stream returned by Any can be early-terminated if it can find the qualified value in the middle
            proxy.Any(num => num % 2 == 0).Subscribe(
                flag => Console.WriteLine("\t!!! existence? {0} !!!", flag),
                () =>
                {
                    Console.WriteLine("\t!!! 'Any' stream is completed !!!");
                    Helper.Pause();
                });

            proxy.Connect();

            new ManualResetEvent(false).WaitOne();
        }

        /// <summary>
        /// Aggregate only publish out its final result, median result won't be published out
        /// </summary>
        private static void TestAggregate()
        {
            IObservable<long> source = Observable.Interval(TimeSpan.FromMilliseconds(150)).Take(10);
            IObservable<long> aggregated = source.Aggregate((sum, current) => sum + current);

            var endEvent = new ManualResetEventSlim(false);
            aggregated.Subscribe(num => Console.WriteLine("sum={0}", num), endEvent.Set);
            endEvent.WaitHandle.WaitOne();
        }

        private static void TestScan()
        {
            IObservable<long> source = Observable.Interval(TimeSpan.FromMilliseconds(100)).Take(10);

            IObservable<long> scanned = source.Scan((sum, current) => sum + current);

            var endEvent = new ManualResetEventSlim(false);
            scanned.Subscribe(num => Console.WriteLine("current sum={0}", num), endEvent.Set);
            endEvent.WaitHandle.WaitOne();
        }

        [Test]
        public static void TestTakeSkip()
        {
            IObservable<int> source = Observable.Range(1, 10);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3 }, source.Take(3).ToEnumerable());
            CollectionAssert.AreEqual(new int[] { 9, 10 }, source.Skip(8).ToEnumerable());
        }

        private static void TestTakeUntil()
        {
            IObservable<long> timeoutEvent = Observable.Timer(TimeSpan.FromSeconds(5));

            IObservable<long> source = Observable.Interval(TimeSpan.FromSeconds(1));
            source.TakeUntil(timeoutEvent).Subscribe(new ConsolePrintObserver<long>());

            Helper.Pause();
        }

        [Test]
        public static void TestDistinctUntilChanged()
        {
            IObservable<int> source = new int[] { 1, 1, 2, 2, 3, 3, 3, 3, 4, 4, 5 }.ToObservable().DistinctUntilChanged();
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5 }, source.ToEnumerable());
        }

        [Test]
        public static void TestMaterialize()
        {
            var source = from notification in Observable.Range(1, 3).Materialize()
                         select Tuple.Create(notification.Kind, notification.HasValue ? notification.Value : -1);
            var expected = new Tuple<NotificationKind, int>[]
            {
                Tuple.Create(NotificationKind.OnNext,1),
                Tuple.Create(NotificationKind.OnNext,2),
                Tuple.Create(NotificationKind.OnNext,3),
                Tuple.Create(NotificationKind.OnCompleted,-1)
            };
            CollectionAssert.AreEqual(expected, source.ToEnumerable());
        }

        /// <summary>
        /// previously, this method is named as "Run", it can be used to execute an asynchronous stream synchronously
        /// it will be blocked untilt the asynchronous stream fire "OnComplete"
        /// </summary>
        private static void TestForEach()
        {
            IObservable<long> source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(4);

            Console.WriteLine("begin ForEach, ......");
            source.ForEach(num => Console.WriteLine("\t{0}", num));
            Console.WriteLine("!!! block until completed !!!");
        }

        /// <summary>
        /// chekanote: I think, Repeat is much more meaningful for Cold Observables than for Hot Observables
        /// you can specify a number for Repeat, that number means how many times that observable will be subscribed
        /// so for cold observables, that means how many times the observable will be started
        /// (for example, if the cold observable is about sending Web Service request, that means the total times
        /// that request will be repeated)
        /// </summary>
        [Test]
        public static void TestRepeat()
        {
            // it will be subscribed twice, so the data will be generated twice
            IObservable<int> source = Observable.Range(1, 3).Repeat(2);
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 1, 2, 3 }, source.ToEnumerable());
        }

        public static void TestRepeat2()
        {
            // chekanote: repeat 3 times, meaning that being subscribed 3 times
            // but only the last "Completed" will be fired out
            IObservable<int> stream = Observable.Create<int>(observer =>
            {
                Console.WriteLine("************** being subscribed.");

                observer.OnNext(100);
                observer.OnCompleted();

                return Disposable.Empty;
            }).Repeat(3);

            stream.Subscribe(Console.WriteLine, () => Console.WriteLine("!!!!! finished !!!!!"));
        }

        public static void TestMain()
        {
            // TestAny();
            // TestFirst_SyncSource();
            TestFirst_AsyncSource();
            // TestFirst_Empty();
            // TestAggregate();
            // TestScan();
            // TestMaterialize();
            // TestForEach();
            // TestRepeat2();
            // TestTakeUntil();
        }
    }
}
