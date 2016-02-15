using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using NUnit.Framework;

namespace PlayRx
{
    [TestFixture]
    sealed class TestGenerateObservable
    {
        private RecordObserver m_observer;

        [SetUp]
        public void Setup()
        {
            m_observer = new RecordObserver();
        }

        [Test]
        public void TestEmpty()
        {
            IObservable<int> empty = Observable.Empty<int>();
            empty.Subscribe(m_observer);

            Assert.IsNull(m_observer.LastValue);
            Assert.IsTrue(m_observer.IsCompleted);
            Assert.IsNull(m_observer.Error);
            Assert.AreEqual(0, m_observer.NumOfNextInvoke);
        }

        [Test]
        public void TestReturn()
        {
            IObservable<int> singular = Observable.Return(88);
            singular.Subscribe(m_observer);

            Assert.AreEqual(1, m_observer.NumOfNextInvoke);
            Assert.AreEqual(88, m_observer.LastValue);
            Assert.IsTrue(m_observer.IsCompleted);
            Assert.IsNull(m_observer.Error);
        }

        [Test]
        public void TestThrow()
        {
            IObservable<int> errored = Observable.Throw<int>(new Exception("test"));
            errored.Subscribe(m_observer);

            Assert.AreEqual(0, m_observer.NumOfNextInvoke);
            Assert.IsNull(m_observer.LastValue);
            Assert.IsFalse(m_observer.IsCompleted);
            Assert.AreEqual("test", m_observer.Error.Message);
        }

        private static void TestRange()
        {
            IObservable<int> range = Observable.Range(1, 10);
            range.Subscribe(new ConsolePrintObserver<int>());
        }

        private static void TestGenerate()
        {
            IObservable<string> basic = Observable.Generate("*", str => str.Length < 6, str => str + "*", str => str);
            basic.Subscribe(new ConsolePrintObserver<string>());

            // -------------------- timer involed, concurrency is imported
            TimeSpan interval = TimeSpan.FromSeconds(1);
            IObservable<string> deplayed = Observable.Generate("y", str => str.Length < 6, str => str + "y",
                                                               str => str.ToUpper(),
                                                               str => interval);
            deplayed.Subscribe(new ConsolePrintObserver<string>());

            Console.ReadLine();
        }

        private static void TestToObservable_BlockMainThread()
        {
            IObservable<int> sequence = Helper.DelayBlockEnumerable(1, 4, TimeSpan.FromSeconds(1)).ToObservable();

            // start iterating the original enumerable on the main thread and block it
            sequence.Subscribe(new ConsolePrintObserver<int>());

            // source and target block the main thread
            Console.WriteLine("*** main thread: subscribed, and also sequence is completed ***");
        }

        private static void TestToObservable_Concurrent()
        {
            IObservable<int> sequence = Helper.DelayBlockEnumerable(1, 10, TimeSpan.FromSeconds(1))
                .ToObservable(Scheduler.ThreadPool);

            // start iterating the original enumerable on another thread, so main thread will not blocked
            sequence.Subscribe(new ConsolePrintObserver<int>());

            Console.WriteLine("*** main thread: subscribed, but sequence is still running ***");

            Helper.Pause();
        }

        private static void TestGenerateWithInterval()
        {
            var source = new string[] { "cheka", "stasi", "kgb" }
                .ToObservableWithInterval(TimeSpan.FromSeconds(2))
                .TimeInterval();
            source.Subscribe(
                item => Console.WriteLine("value='{0}',interval='{1}'", item.Value, item.Interval),
                () => Console.WriteLine("!!! completed !!!"));

            Helper.Pause();
        }

        /// <summary>
        /// start itself include Asynchronous feature, it will execute the job asynchronously
        /// (it is easy to comprehend, synchronous job don't need Rx to notify result and completion
        /// so there is no need to create a Sync-version of Start)
        /// </summary>
        private static void TestStart()
        {
            IObservable<Unit> actionSource = Observable.Start(() => Thread.Sleep(TimeSpan.FromSeconds(1)));
            actionSource.Subscribe(_ => { }, () => Console.WriteLine("\tcompleted"));
            Console.WriteLine("'Subscribe' NOT blocked by observer");
            Helper.Pause();

            IObservable<int> funcSource = Observable.Start(() =>
                                                               {
                                                                   Thread.Sleep(TimeSpan.FromSeconds(1));
                                                                   return 88;
                                                               });
            funcSource.Subscribe(num => Console.WriteLine("\tresult={0}", num),
                                 () => Console.WriteLine("\tcompleted"));
            Console.WriteLine("'Subscribe' NOT blocked by observer");
            Helper.Pause();
        }

        private static void TestToEnumerable()
        {
            IObservable<Timestamped<long>> source = Observable.Interval(TimeSpan.FromSeconds(1)).Timestamp().Take(5);
            Console.WriteLine("non-blocked observable source, ......");

            IEnumerable<Timestamped<long>> sequence = source.ToEnumerable();
            foreach (Timestamped<long> tlong in sequence)
            {
                Console.WriteLine("{0}-th: {1}", tlong.Value, tlong.Timestamp.LocalDateTime.ToMinSecString());
            }
        }

        public static void TestMain()
        {
            // TestRange();
            // TestGenerate();
            // TestToObservable_BlockMainThread();
            // TestToObservable_ConcurrentSource();
            // TestGenerateWithInterval();
            TestStart();
            // TestToEnumerable();
        }
    }
}
