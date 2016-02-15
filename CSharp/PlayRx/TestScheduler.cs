using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Threading;

namespace PlayRx
{
    static class TestScheduler
    {
        private static void InspectGeneration(int num)
        {
            Console.WriteLine("[threadid={0}] <{1}> is generated.", Thread.CurrentThread.ManagedThreadId, num);
        }

        public static void MockLongOperation(int num)
        {
            int threadid = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("\t[threadid={0}] working on: {1}", threadid, num);
            Thread.Sleep(1000);
            Console.WriteLine("\t[threadid={0}] finished: {1}", threadid, num);
        }

        private static void TestDefault_Immediate()
        {
            IObservable<int> source = Enumerable.Range(1, 4).ToObservable();

            // note: code observable starts when subscribe, and block until 
            // all the callbacks are executed
            // Immediate Scheduler also block the generation of next data
            // not only each callback in "Subscribe" is executely in sequence, 
            // note: but "Do" and subscribed callback are also executed in sequence
            source
                .Do(InspectGeneration)
                .Subscribe(MockLongOperation);
            Console.WriteLine("!!! ALL DONE !!!");
        }

        /// <summary>
        /// note: for Scheduler.ThreadPool,Scheduler.TaskPool,Scheduler.NewThread
        /// they all exhibit asynchronous feature
        /// "subscribed callback" doesn't block the "data generation"
        /// "data generation" and "subscribed callbacks" are executed concurretly
        /// note: but each "subscribed callback" is executed in sequence, NEVER overlap
        /// </summary>
        private static void TestObserveOn_NonBlocking(IScheduler scheduler)
        {
            IObservable<int> source = Enumerable.Range(1, 4).ToObservable();

            source
                .Do(InspectGeneration)
                .ObserveOn(scheduler)
                .Subscribe(MockLongOperation);

            Console.WriteLine("### code observable started ###");
            Console.ReadLine();// since callbacks are running asynchronously, so I have to block by "readline"
        }

        private static void RunGenerateCallbackInSequence(IScheduler generateScheduler, IScheduler callbackScheduler)
        {
            IEnumerable<int> integers = Enumerable.Range(1, 4);

            IObservable<int> source = generateScheduler == null ? integers.ToObservable() : integers.ToObservable(generateScheduler);
            source = source.Do(InspectGeneration);

            IObservable<int> publishing = callbackScheduler == null ? source : source.ObserveOn(callbackScheduler);
            publishing.Subscribe(MockLongOperation);

            Console.WriteLine("### Blocks until ALL DONE ###\n");
        }

        // note: below two samples are very important, because Enumerable.ToObservable, the data source
        // can be a infinite enumerable sequence, so the default scheduler of "ToObservable" is already "CurrentThread"
        // so if both generate scheduler and callback scheduler is "CurrentThread", then callback is prior then "MoveNext"
        // so still "generation" and "callback" are executed in sequence, slow callback will block next data generation
        // so you can let the data generated "immediately", by specifying generation scheduler to be "Immediate"
        // this way the next "MoveNext" will not be appended to the last position of the current thread, but be executed
        // by recursion, so all "callbacks" are appended in the tail of the current thread,
        // and will only start after all data are generated
        private static void TestBlocking()
        {
            // data are generated recursively, but callbacks are queued
            // so slow callback doesn't block the data generation
            RunGenerateCallbackInSequence(Scheduler.Immediate, Scheduler.CurrentThread);

            // the default scheduler for "ToObservable" is still "CurrentThread", which put the action
            // to be scheduled at the end of the current thread
            // both data generation and callback are scheduled in current thread, so slow callback block data generation
            RunGenerateCallbackInSequence(null, Scheduler.CurrentThread);
        }

        private static void TestConcurrentDiff_At_Sender_Receiver()
        {
            // note: although 'Scheduler.ThreadPool' is used, but "data generation" and "callback"
            // are still executed in the same scheduler, so slow callback still block the data generation
            IObservable<int> stream = Enumerable.Range(1, 5).ToObservable(Scheduler.ThreadPool);
            using (stream.Do(InspectGeneration)
                .Subscribe(MockLongOperation))
            {
                Console.WriteLine("Press any key to continue, ......");
                Console.ReadLine();
            }

            // cold observable restarts from beginning
            using (stream.Do(InspectGeneration)
                .ObserveOn(Scheduler.ThreadPool)
                .Subscribe(MockLongOperation))
            {
                Console.WriteLine("Press any key to continue, ......");
                Console.ReadLine();
            }
        }

        private static void ScheduleTasks(IScheduler scheduler)
        {
            Action leafAction = () => Console.WriteLine("\t\tleaf action");

            Action innerAction = () =>
                                     {
                                         Console.WriteLine("\tinner action starts.");
                                         scheduler.Schedule(leafAction);
                                         Console.WriteLine("\tinner action ends.");
                                     };

            Action outerAction = () =>
                                     {
                                         Console.WriteLine("outer action starts.");
                                         scheduler.Schedule(innerAction);
                                         Console.WriteLine("outer action ends.");
                                     };

            scheduler.Schedule(outerAction);
        }

        private static void CheckScheduleTask()
        {
            Console.WriteLine("************** Use Immediate Scheduler, ......");
            ScheduleTasks(Scheduler.Immediate);

            Console.WriteLine("\n************** Use CurrentThread Scheduler, ......");
            ScheduleTasks(Scheduler.CurrentThread);
        }

        private static void TestNewThreadScheduler()
        {
            // the NewThread Scheduler specified in the ToObservable indicates
            // chekanote: spawn a new thread to iterate all the values in the Enumerable
            // not means to spawn a new thread to process each value in the Enumerable, so only one thread is created and used
            IObservable<int> source = Enumerable.Range(1, 5).ToObservable(Scheduler.NewThread);

            // chekanote: but this NewThread scheduler is supposed to be meaning "spawn new thread for each OnNext"
            // but at least in this demo, it uses the same thread repeatedly to invoke each OnNext
            source.Do(InspectGeneration).ObserveOn(Scheduler.NewThread).Subscribe(MockLongOperation);

            Helper.Pause();
        }

        public static void TestMain()
        {
            // TestDefault_Immediate();
            // TestObserveOn_NonBlocking(Scheduler.ThreadPool);
            // TestObserveOn_NonBlocking(Scheduler.TaskPool);
            // TestBlocking();
            TestConcurrentDiff_At_Sender_Receiver();
            // CheckScheduleTask();
            // TestNewThreadScheduler();
        }// TestMain
    }
}
