using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Diagnostics;

namespace PlayRx
{
    sealed class TestConcurrency
    {
        #region "inner classes"

        sealed class BlockObserver<T> : IObserver<Timestamped<T>>
        {
            private readonly TimeSpan m_blockInterval;

            public BlockObserver(TimeSpan blockInterval)
            {
                m_blockInterval = blockInterval;
            }

            public void OnNext(Timestamped<T> tvalue)
            {
                Console.WriteLine("[{0,-2}] ProduceTime={1},ConsumeTime={2}",
                    tvalue.Value,
                    tvalue.Timestamp.LocalDateTime.ToMinSecString(),
                    DateTime.Now.ToMinSecString());

                Thread.Sleep(m_blockInterval);
            }

            public void OnError(Exception error)
            {
                throw error;
            }

            public void OnCompleted()
            {
                Console.WriteLine("!!! completed !!!");
            }
        }

        #endregion

        #region "methods"

        #region 'serialized execution feature'

        public static long SumUp(int length, IScheduler scheduler = null)
        {
            // 'Interval' starts from 0
            IObservable<long> source = Observable.Interval(TimeSpan.FromMilliseconds(200))
                .Take(length)
                .Do(num => Console.WriteLine("'{0}' generated.", num));

            var endEvent = new ManualResetEvent(false);
            long total = 0;

            source = scheduler == null ? source : source.ObserveOn(scheduler);
            source.Subscribe(
                num =>
                {
                    Console.WriteLine("\tbegin processing '{0}',......", num);

                    // sleep long time, IF run in parallel, this will increate race opportunity
                    Thread.Sleep(TimeSpan.FromSeconds(1));

                    // chekanote: since observer are executed in sequence, there is no need to lock
                    total += num;
                },
                () => endEvent.Set());

            endEvent.WaitOne();

            return total;
        }

        private static void NoSyncNeed_SynScheduler()
        {
            long total = SumUp(5);

            Debug.Assert(total == 10);

            Console.WriteLine("test succeeds !");
        }

        private static void NoSyncNeed_AsyncScheduler()
        {
            long total = SumUp(4, Scheduler.TaskPool);

            Debug.Assert(total == 6);

            Console.WriteLine("test succeeds.");
        }

        #endregion

        /// <summary>
        /// chekanote: the effect is that, the ProduceTime and the ConsumeTime is nearly the same
        /// which proves that in below codes, slow observer block the data source
        /// this is because, Rx always guarantee that "observer are executed in sequence, in serialized fashion"
        /// no matter which scheduler is used
        /// in below codes, concurrency is imported at the source, then all its following operators will executed on 
        /// the same thread as the source. observer and data generation share the same thread from the scheduler
        /// then Observer's slow OnNext will block the next data generation
        /// as a result, concurrency is totally wasted
        /// </summary>
        private static void ImportConcurrencyAtSource()
        {
            IObservable<Timestamped<int>> source = Observable.Range(1, 5, Scheduler.TaskPool).Timestamp();
            source.Subscribe(new BlockObserver<int>(TimeSpan.FromSeconds(3)));

            Helper.Pause();
        }

        /// <summary>
        /// chekanote: the effect is that all the ProduceTime is nearly the same, and ConsumeTime is intervalled as expected
        /// which proves that the source isn't blocked by the slow observer
        /// because by importing cnocurrency right before the target, the source observable consider putting job
        /// into the Scheduler as the completion, so the source and Observer can executed on different threads
        /// so the source is not blocked at all. Concurrency is used correctly
        /// (fast job is queued the scheduler for slow observer to execute them one by one)
        /// </summary>
        private static void ImportConcurrencyAtTarget()
        {
            IObservable<Timestamped<int>> source = Observable.Range(1, 5).Timestamp();

            source.ObserveOn(Scheduler.TaskPool).Subscribe(new BlockObserver<int>(TimeSpan.FromSeconds(3)));

            Helper.Pause();
        }

        public static void TestMain()
        {
            // ImportConcurrencyAtSource();
            // ImportConcurrencyAtTarget();
            NoSyncNeed_SynScheduler();
            NoSyncNeed_AsyncScheduler();
        }

        #endregion
    }
}
