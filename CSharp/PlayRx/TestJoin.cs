using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace PlayRx
{
    static class TestJoin
    {
        private static IEnumerable<string> GetConsoleInput()
        {
            while (true)
            {
                yield return Console.ReadLine();
            }
        }

        private static void TestJoin_PointEvent()
        {
            IObservable<long> leftStream = Observable.Interval(TimeSpan.FromSeconds(2));

            // chekanote: I don't want to use "Scheduler.NewThread", I don't know why the author in WorkShop
            // want to use "NewThread", which will fire each OnNext by spawning a totally new thread
            // (NOT using one thread to iterate all the values, but fire each OnNext in its seperate thread)
            // chekanote: and "EventLoopScheduler" will create a new thread and schedule all the jobs on that "designated" thread
            IObservable<string> rightStream = GetConsoleInput().ToObservable(new EventLoopScheduler());

            IObservable<string> joined = leftStream.Join(
                rightStream,
                left => Observable.Timer(TimeSpan.FromSeconds(5)),
                right => Observable.Empty<Unit>(),
                (num, str) => string.Format("\t'{0}' is in window<{1}>", str, num));

            joined.ForEach(txt=>Console.WriteLine(txt));
        }

        /// <summary>
        /// the stream ends when has new comers
        /// </summary>
        private static void TestJoin_EndWhenNewerComes()
        {
            IObservable<long> leftStream = Observable.Interval(TimeSpan.FromSeconds(2));

            IObservable<string> rightStream = GetConsoleInput().ToObservable(new EventLoopScheduler());

            // chekanote: here use "Publish+Selector", the parameter within selector "hogRight" is a hot observable
            // which can be used multiple times within the selector without any extra side-effect (caused by multiple subscription)
            IObservable<string> joined = rightStream.Publish(hotRight =>
                leftStream.Join(
                hotRight,
                num => Observable.Timer(TimeSpan.FromSeconds(5)),
                right => hotRight,
                (num, str) => string.Format("\t'{0}' is in window<{1}>", str, num)));

            joined.ForEach(txt=>Console.WriteLine(txt));
        }

        public static void TestMain()
        {
            // TestJoin_PointEvent();
            TestJoin_EndWhenNewerComes();
        }
    }
}
