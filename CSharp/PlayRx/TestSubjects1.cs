using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace PlayRx
{
    static class TestSubjects1
    {
        private static int SubscriberCounter = 0;

        private static void AttachConsoleHandlers<T>(this IObservable<T> subject)
        {
            int subscriberId = ++SubscriberCounter;
            subject.Subscribe(
                value => Console.WriteLine("Subscriber[{0}]: {1}", subscriberId, value),
                () => Console.WriteLine("Subscriber[{0}]: Completed", subscriberId));
        }

        static class TestSubject
        {
            public static void TestSimple()
            {
                ISubject<int> subject = new Subject<int>();
                subject.AttachConsoleHandlers();

                subject.OnNext(1);
                subject.OnNext(2);
                subject.OnCompleted();
            }

            public static void TestStatelessFeature()
            {
                ISubject<int> subject = new Subject<int>();
                subject.OnNext(1);// ignored

                subject.AttachConsoleHandlers();
                subject.OnNext(2);
                subject.OnCompleted();
            }

            public static void TestOrderContract()
            {
                ISubject<int> subject = new Subject<int>();
                subject.AttachConsoleHandlers();

                subject.OnNext(0);
                subject.OnCompleted();

                // chekanote: subject will comply automatically with Rx's contract, that no message will published out
                // after completion or error
                subject.OnNext(1);
                subject.OnNext(2);
            }

            /// <summary>
            /// chekanote: use default scheduler, both data source and data sink are executed on the same thread
            /// block callback not even block the execution of next data, it EVEN prevent the source to produce next data
            /// chekanote: Rx is a push model, by default, the callback are invoked by the sender
            /// on the same thread where the sender runs
            /// </summary>
            public static void TestDefaultScheduler()
            {
                ISubject<int> subject = new Subject<int>();
                subject.Subscribe(Helper.BlockResponse);

                int[] inputs = new int[] { 1, 2, 3 };
                foreach (int input in inputs)
                {
                    Console.WriteLine("call OnNext with '{0}'", input);
                    subject.OnNext(input);
                }
            }

            public static void TestSubscribeOn()
            {
                ISubject<int> subject = new Subject<int>();
                subject.SubscribeOn(Scheduler.NewThread).Subscribe(Helper.BlockResponse);

                // chekanote: below code is very important, without below code, there is huge possiblity
                // even after all values are passed into "OnNext", but at that time, that new thread
                // is still not started yet, so no subscription is made, so no callback will be fired

                // !!!!!!! chekanote: there are two ways to prove the theory above:
                // !!!!!!! chekanote: Method 1: uncomment below code, after that short period, a new thread has already
                // !!!!!!! been spawned, and then subscription is made, then when data production begins
                // !!!!!!! there is already observer listening, so callback will be fired
                // Thread.Sleep(100);

                // !!!!!!! chekanote: Method 2: you give it a long list of data to be generated
                // !!!!!!! you can see that without "sleep" between "SubscribeOn().Subscribe()" and data generation
                // !!!!!!! there is race condition between the thread which makes the subscription and the main thread
                // !!!!!!! so the observer will miss the first several data

                // !!!!!!! chekanote: "SubscribeOn" can control on which thread the subscription is made
                // !!!!!!! since Cold Observable begins generating the data everytime being subscribed
                // !!!!!!! so for Cold Observable, by using SubscribeOn, you can control the thread where data is generated
                // !!!!!!! so "SubscribeOn" only make sense for Cold Observable, in this case, "Subject" is hot
                // !!!!!!! it won't have any special effect

                // !!!!!!! in this case, by using "SubscribeOn", it only control which thread is used to
                // !!!!!!! make the subscription, but after subscription, it is still the MainThread is used to generate
                // !!!!!!! the data, to push out the data, so still callback will be executed on mainthread
                // !!!!!!! chekanote: so in this case, callback and data generation are still on the same thread, blocking callback
                // !!!!!!! will still blockk the data generation
                IEnumerable<int> inputs = Enumerable.Range(1, 30);
                foreach (int input in inputs)
                {
                    Console.WriteLine("call OnNext with '{0}'", input);
                    subject.OnNext(input);
                }

                // chekanote: only subscription is made on another thread
                // after subscription, both data generation and observer run on the main thread
                // until all data are published out, so the whole program will be blocked (no need to "Console.ReadLine()")
            }

            /// <summary>
            /// chekanote: since callbacks are executed on a different thread, although these callbacks are still
            /// executed in sequence, never overlap, but slow callback will not block the data production any more
            /// </summary>
            public static void TestObserveOn()
            {
                ISubject<int> subject = new Subject<int>();
                subject.ObserveOn(Scheduler.ThreadPool).Subscribe(Helper.BlockResponse);

                int[] inputs = new int[] { 8, 9, 10 };
                foreach (int input in inputs)
                {
                    Console.WriteLine("call OnNext with '{0}'", input);
                    subject.OnNext(input);
                }
                new ManualResetEvent(false).WaitOne();
            }
        }

        static class TestReplaySubject
        {
            public static void TestStateFeature()
            {
                ISubject<int> subject = new ReplaySubject<int>();
                subject.OnNext(1);
                subject.OnNext(2);

                subject.AttachConsoleHandlers();

                Helper.Pause();
                subject.OnNext(3);
                subject.AttachConsoleHandlers();

                Helper.Pause();
                subject.OnCompleted();
            }
        }

        static class TestBehaviorSubject
        {
            public static void TestAlwaysLast()
            {
                ISubject<int> subject = new BehaviorSubject<int>(1);// default value
                subject.AttachConsoleHandlers();

                Helper.Pause();
                subject.OnNext(3);// different from AsyncSubject, data is published out even the stream is not completed
                subject.AttachConsoleHandlers();

                Helper.Pause();
                subject.OnCompleted();
            }
        }

        static class TestAsyncSubject
        {
            /// <summary>
            /// nothing printed on the screen because "OnCompleted" is not invoked
            /// </summary>
            public static void TestNoCompleteNoPub()
            {
                ISubject<int> stream = new AsyncSubject<int>();
                stream.AttachConsoleHandlers();

                stream.OnNext(1);
                stream.OnNext(2);
            }

            public static void TestOnlyPubLastWhenCompleted()
            {
                ISubject<int> stream = new AsyncSubject<int>();
                stream.AttachConsoleHandlers();

                foreach (int num in Enumerable.Range(1, 3))
                {
                    Console.WriteLine("\tdata generated: {0}", num);
                    stream.OnNext(num);
                }

                Helper.Pause();
                stream.OnCompleted();
            }

            public static void TestStateFeature()
            {
                ISubject<int> stream = new AsyncSubject<int>();
                stream.OnNext(1);
                stream.OnNext(10);
                stream.OnCompleted();

                stream.AttachConsoleHandlers();
                stream.AttachConsoleHandlers();
            }
        }

        public static void TestMain()
        {
            // TestSubject.TestSimple();
            // TestSubject.TestStatelessFeature();
            // TestSubject.TestOrderContract();
            // TestSubject.TestDefaultScheduler();
            // TestSubject.TestObserveOn();
            // TestSubject.TestSubscribeOn();

            // TestReplaySubject.TestStateFeature();

            TestBehaviorSubject.TestAlwaysLast();

            // TestAsyncSubject.TestNoCompleteNoPub();
            // TestAsyncSubject.TestOnlyPubLastWhenCompleted();
            // TestAsyncSubject.TestStateFeature();
        }
    }
}
