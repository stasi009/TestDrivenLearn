using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace PlayRx
{
    static class TestTimeRelated
    {
        private static void TestTimestamp()
        {
            var source = from tmstamped in Helper.DelayBlockEnumerable(1, 10, TimeSpan.FromSeconds(1)).ToObservable().Timestamp()
                         select string.Format("[{0}]: {1}", tmstamped.Timestamp, tmstamped.Value);
            source.Subscribe(new ConsolePrintObserver<string>());
        }

        /// <summary>
        /// timeout only make effect when OnNext isn't called after a specific time
        /// chekanote: it doesn't control the whole timecost of a sequence
        /// but only control how long should the observer wait for the next data
        /// if the observer wait enough long but still don't get the next piece of data, 
        /// the timeout-observer will push an exception to the observer
        /// </summary>
        private static void TestTimeout()
        {
            IObservable<string> source = Helper.MakeConsoleInputObservable().Timeout(TimeSpan.FromSeconds(3));
            source.Subscribe(new ConsolePrintObserver<string>());
        }

        /// <summary>
        /// this test shows that the frequency of output is still decided by the slower part of the producer and consumer
        /// chekanote: by default, slow consumer will still block the producer
        /// </summary>
        private static void CheckInterval(int produceInterval, int consumeInterval, IScheduler scheduler)
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(produceInterval))
                .Timestamp();

            source = scheduler == null ? source : source.ObserveOn(scheduler);

            using (source.Subscribe(tlong =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(consumeInterval));
                Console.WriteLine("ProduceTime='{0}',ConsumeTime='{1}'",
                    tlong.Timestamp.LocalDateTime.ToLongTimeString(),
                    DateTime.Now.ToLongTimeString());
            }))
            {
                Helper.Pause();
            }
        }

        private static void TestInterval()
        {
            // chekanote: producer is faster than consumer, timestamp is decided by consumer
            // actually producer is blocked by consumer
            // CheckInterval(1, 2, null);

            // producer is slower than consumer, timestamp is decided by producer
            // CheckInterval(3, 1, null);

            // when consumer is slower than producer, actually consumer will block producer
            // for non-blocking purpose, slow consumer must run on another thread from producer
            CheckInterval(1, 4, Scheduler.TaskPool);
        }

        private static void TestDelay_RelativeTimespan()
        {
            var delayed = Observable.Interval(TimeSpan.FromSeconds(1))
                .Take(10)
                .Timestamp()
                .Delay(TimeSpan.FromSeconds(3));

            CheckDelay(delayed);
        }

        private static void TestDelay_AbsoluteTime()
        {
            var futureTime = DateTime.Now.AddSeconds(3);

            var delayed = Observable.Interval(TimeSpan.FromSeconds(1))
                .Take(5)
                .Timestamp()
                .Delay(futureTime);

            CheckDelay(delayed);
        }

        private static void CheckDelay(IObservable<Timestamped<long>> delayed)
        {
            delayed.Subscribe(
                tlong =>
                {
                    DateTime consumeTime = DateTime.Now;
                    DateTime produceTime = tlong.Timestamp.LocalDateTime;
                    TimeSpan difference = consumeTime - produceTime;
                    Console.WriteLine("<{0}> delayed '{1}' seconds", tlong.Value, difference.TotalSeconds);
                },
                () => Console.WriteLine("finished."));

            Helper.Pause();
        }

        private static void TestSample()
        {
            IObservable<TimeInterval<long>> source = Observable.Interval(TimeSpan.FromSeconds(0.2))
                .Sample(TimeSpan.FromSeconds(1))
                .TimeInterval();

            source.Subscribe(tlong => Console.WriteLine("value={0},interval={1}", tlong.Value, tlong.Interval.TotalSeconds));

            Helper.Pause();
        }

        private static void TestThrottle1(TimeSpan produceInterval, TimeSpan throttleInterval)
        {
            var source = Observable.Interval(produceInterval).Throttle(throttleInterval);
            source.Subscribe(Console.WriteLine);

            Helper.Pause();
        }

        private static void TestThrottle2()
        {
            IObservable<int> source = Observable.Create<int>((observer) =>
            {
                for (int index = 0; index < 100; index++)
                {
                    observer.OnNext(index);
                    Thread.Sleep(index % 10 < 5 ? 100 : 300);
                }
                observer.OnCompleted();
                return Disposable.Empty;
            });

            // chekanote: since the data production when "index%10 < 5" is fast, 0~4 will never appear in the throttled stream
            source.Throttle(TimeSpan.FromMilliseconds(200)).Subscribe(Console.WriteLine);
        }

        private static void CheckTimer(this IObservable<Timestamped<long>> stream)
        {
            Console.WriteLine("******** started at <{0}> ********", DateTime.Now.ToMinSecString());
            stream.ForEach(tvalue =>
                Console.WriteLine("timstamp={0}, value={1}", tvalue.Timestamp.LocalDateTime.ToMinSecString(), tvalue.Value));
        }

        /// <summary>
        /// Observable.Interval is a simple wrapper around Observable.Timer.
        /// </summary>
        private static void TestTimer_Repeat()
        {
            IObservable<Timestamped<long>> stream = Observable.Timer(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(1)).Timestamp();
            stream.CheckTimer();
        }

        private static void TestTimer_OneShot()
        {
            IObservable<Timestamped<long>> stream = Observable.Timer(TimeSpan.FromSeconds(2)).Timestamp();
            stream.CheckTimer();
        }

        public static void TestMain()
        {
            // TestTimestamp();
            // TestTimeout();
            // TestInterval();
            // TestDelay_RelativeTimespan();
            // TestDelay_AbsoluteTime();
            // TestSample();
            // !!! the publisher is much more faster than the Throttled threshold, then throttled stream will never get any value
            // TestThrottle1(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200));
            // TestThrottle1(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(100));
            // TestThrottle2();
            // TestTimer_Repeat();
            TestTimer_OneShot();
        }
    }
}
