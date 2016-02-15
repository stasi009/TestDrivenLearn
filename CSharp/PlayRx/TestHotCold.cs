using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Linq;

namespace PlayRx
{
    public class TestHotCold
    {
        // the test demonstrate when the cold Observable is started
        // the cold observable won't start when "intermediate transformer" (such filter, mapper, etc) is connected
        // it will only start when the final subscriber is started
        private static void WhenColdObservableStart()
        {
            IObservable<long> oriStream = Observable.Interval(TimeSpan.FromSeconds(1));

            // even Select has subscribed to the original stream, however, at this time, since there is no "final subscriber"
            // so the whole cold stream hasn't started yet
            var mappedStream =  oriStream.Do(n => Console.WriteLine("--- {0} is produced", n)).Select(n=>n*n);
            Helper.Pause();

            mappedStream.Subscribe(n => Console.WriteLine("{0} is sinked and processed", n));
            Helper.Pause();
        }

        /// <summary>
        /// two distinctive conceptions:Stream and Observable instance
        /// for hot observables, they are the same
        /// for cold observables, they are not. Cold Observables are the factory of Streams
        /// everytime the cold observables are subscribed, it will generate a new stream
        /// </summary>
        private static void ShowColdFeature()
        {
            IObservable<long> intervals = Observable.Interval(TimeSpan.FromSeconds(1));

            // Observable.Interval is a cold one, total STATE-LESS
            IDisposable subscription1 = intervals.Subscribe(new ConsolePrintObserver<long>());
            Helper.Pause();

            // note: the second subscriber will get a totally new sequence, it will also start with 0. 
            IDisposable subscription2 = intervals.Subscribe(new ConsolePrintObserver<long>("p", ConsoleColor.Green));
            Helper.Pause();
            subscription2.Dispose();

            Helper.Pause();
            subscription1.Dispose();
        }

        /// <summary>
        /// publish is used to convert Cold =====> Hot
        /// </summary>
        private static void TestPublish()
        {
            // what "Publish" returns is just a proxy, or say, a subject, which serves 
            // both as Observer (to original cold observable) and Observable (to observer connects to it)
            IConnectableObservable<long> proxy = Observable.Interval(TimeSpan.FromSeconds(1)).Publish();

            // the proxy subscribe on the original source cold observable, data production starts
            IDisposable connection = proxy.Connect();

            // when the first real observer connects, it already cannot receive 0 anymore
            Helper.Pause("subscribe 1st");
            IDisposable subscription1 = proxy.Subscribe(new ConsolePrintObserver<long>("one"));

            Helper.Pause("subscribe 2nd");
            IDisposable subscription2 = proxy.Subscribe(new ConsolePrintObserver<long>("two", ConsoleColor.Magenta));

            // since proxy is unsubscribed, all its follower will not receive any data any more
            Helper.Pause("disconnect");
            connection.Dispose();
        }

        private static void TestConnectMultipleTimes()
        {
            IConnectableObservable<long> proxy = Observable.Interval(TimeSpan.FromSeconds(0.5)).Publish();
            proxy.Subscribe(new ConsolePrintObserver<long>());
            proxy.Subscribe(new ConsolePrintObserver<long>("q", ConsoleColor.DarkGreen));

            for (int index = 0; index < 2; index++)
            {
                // everytime connected, the proxy subscribed to the underlying cold observable
                // the underlying cold observable will start from beginning once again
                using (proxy.Connect())
                {
                    Helper.Pause();
                }
            }//for
        }

        /// <summary>
        /// the subscription of the real data consumer won't affect the underlying cold observable
        /// it will keep generating data as long as proxy is connected to it
        /// </summary>
        private static void TestUnderlyingStreamAlwaysOpen()
        {
            IConnectableObservable<long> proxy = Observable.Interval(TimeSpan.FromSeconds(0.5))
                .Do(num => Console.WriteLine("<{0}> produced.", num))
                .Publish();

            Helper.Pause("connect");
            IDisposable connection = proxy.Connect();

            Helper.Pause("subscribe observer");
            IDisposable subscription = proxy.Subscribe(num => Console.WriteLine("\t<{0}> consumed.", num));

            Helper.Pause("unsubscribe observer");
            subscription.Dispose();

            Helper.Pause("disconnect");
            connection.Dispose();
        }

        /// <summary>
        /// note: RefCount will be called on a 'IConnectableObservable' provides auto-connect and auto-disconnect feature
        /// actually, it add back some 'cold' feature
        /// 'Returns an observable sequence that stays connected to the source as long as there is 
        /// at least one subscription to the observable sequence.'
        /// it will auto-connect to the underlying source when it has first observer
        /// and it will auto-disconnect to the underlying source when all its observers are subscribed
        /// </summary>
        private static void TestRefCount()
        {
            IObservable<long> autoProxy = Observable.Interval(TimeSpan.FromSeconds(0.5))
                .Do(num => Console.WriteLine("<{0}> produced.", num))
                .Publish()
                .RefCount();

            Helper.Pause("subscribe 1st observer");
            // note: at this time, for the first observer, proxy auto-connect with source
            IDisposable subscription1 = autoProxy.Subscribe(num => Console.WriteLine("\t1st: <{0}> consumed.", num));

            Helper.Pause("subscribe 2nd observer");
            IDisposable subscription2 = autoProxy.Subscribe(num => Console.WriteLine("\t2nd: <{0}> consumed.", num));

            Helper.Pause("unsubscribe 1st");
            subscription1.Dispose();

            Helper.Pause("unsubscribe 2nd");
            // note: at this time, since no observers any more, proxy auto-disconnect from the source
            subscription2.Dispose();
        }

        private static void CheckReplayable(Func<IObservable<long>, IConnectableObservable<long>> proxyFactory)
        {
            IObservable<long> source = Observable.Interval(TimeSpan.FromSeconds(1));
            IConnectableObservable<long> proxy = proxyFactory(source);

            using (proxy.Connect())
            {
                Helper.Pause("subscribe 1st");
                proxy.Subscribe(new ConsolePrintObserver<long>("1st"));

                Helper.Pause("subscribe 2nd");
                proxy.Subscribe(new ConsolePrintObserver<long>("2nd", ConsoleColor.Green));

                Helper.Pause("disconnect");
            }

            Helper.Pause("subscribe after disconnect");
            proxy.Subscribe(new ConsolePrintObserver<long>("late", ConsoleColor.Red));

            Console.WriteLine("**************** END ****************");
        }

        /// <summary>
        /// chekanote: when call "proxy.connect", this proxy connect to underlying source, the data generation already begins
        /// after that, when there is observer subscribing, the value generated before the subscription will not be fired
        /// to the observer, for the observer, those values are completely lost
        /// </summary>
        private static void TestNonReplayableProxy()
        {
            CheckReplayable(source => source.Publish());
        }

        /// <summary>
        /// note: different from calling "Publish" which returns a non-replayble proxy, by calling "Replay" it will return a proxy
        /// which has memory capability
        /// when proxy is connected, the underlying data generation already starts, although there is no observer at that time
        /// but those values generated before any subscriptions are stored to be replayed later
        /// when there is observer subscribed on, all values generated before that subscription will be pushed into the
        /// observer immediately, 
        /// chekanote: so no matter when the subscription occur, the observer can always get all 
        /// the data generated from the very beginning, 
        /// chekanote: EVEN AFTER that proxy is disconnected from the source
        /// </summary>
        private static void TestReplayableProxy()
        {
            CheckReplayable(source => source.Replay());
        }

        /// <summary>
        /// note: from this "Publish with Selector" demo, we can conclude:
        /// 1. inside the selector, the input parameter of that selector is a hot observable. but more like the result of "RefCount"
        /// that hotstream will first connect to its underlying source when the first observer subscribed
        /// then after that, later subscription will not re-start the underlying source
        /// 2. the result from this "Publish with Selector" is a cold one, re-subscribe will re-start the data generation, and will re-run the factory
        /// 3. remember the selector is just a factory, the codes in the selector will only be executed once during initialization (before any subscribers)
        /// chekanote: during runtime, during data publication, it is the returned observer running, not the selector
        /// </summary>
        private static void TestPublishWithSelector()
        {
            IObservable<long> coldSource = Observable.Interval(TimeSpan.FromSeconds(0.5))
                .Publish(hotstream =>
                             {
                                 IObservable<long> source1 = hotstream.Skip(2).Take(4);

                                 // note: any delay within the selector only happens once during the initialization
                                 // such delay won't happen during runtime
                                 Thread.Sleep(TimeSpan.FromSeconds(1));
                                 Console.WriteLine("*** within factory: sub-source1 built");

                                 IObservable<long> source2 = hotstream.Skip(3).Take(2);
                                 Console.WriteLine("*** within factory: sub-source2 built");

                                 return source1.Concat(source2);
                             });

            Helper.Pause("subscribe the first");
            coldSource.Subscribe(new ConsolePrintObserver<long>("1st"));

            Helper.Pause("subscribe the second");
            coldSource.Subscribe(new ConsolePrintObserver<long>("2nd", ConsoleColor.Green, 2));

            Helper.Pause("exit");
        }

        public static void TestMain()
        {
            WhenColdObservableStart();
            // ShowColdFeature();
            // TestPublish();
            // TestConnectMultipleTimes();
            // TestUnderlyingStreamAlwaysOpen();
            // TestRefCount();
            // TestNonReplayableProxy();
            // TestReplayableProxy();
            // TestPublishWithSelector();
        }
    }
}
