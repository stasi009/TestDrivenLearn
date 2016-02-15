using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;

using NUnit.Framework;

namespace PlayRx
{
    static class TestCombination
    {
        // ********************************************************************* //
        #region "inner classes"

        sealed class TmIntervalPrinter<T> : IObserver<TimeInterval<T>>
        {
            public void OnNext(TimeInterval<T> t)
            {
                Console.WriteLine("value='{0}'\tinterval=<{1}>", t.Value, t.Interval.TotalSeconds);
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

        /// <summary>
        /// for Zip, if one of the source streams publish values faster than the other stream, 
        /// the rate of publishing will be dictated by the slower of the two streams.
        /// </summary>
        static class TestZip
        {
            /* 
             * stream1 ----0----1----2|        
             * stream2 --a--b--c--d--e--f|   
             * 
             * result  ----0----1----2|
                           a    b    c
             */
            private static void Check<T1, T2>(IList<T1> list1, IList<T2> list2)
            {
                IObservable<T1> source1 = list1.ToObservableWithInterval(TimeSpan.FromSeconds(0.5));
                IObservable<T2> source2 = list2.ToObservableWithInterval(TimeSpan.FromSeconds(3));

                IObservable<string> zipped = source1.Zip(source2, (x, y) => string.Format("{0}-{1}", x, y));

                zipped.TimeInterval().Subscribe(new TmIntervalPrinter<string>());

                Helper.Pause();
            }

            public static void TestMain()
            {
                // Check(new char[] { 'a', 'b', 'c' }, new int[] { 1, 2, 3 });
                Check(new int[] { 1, 2 }, new string[] { "cheka", "kgb", "stasi" });// stops when the short one completed
            }
        }

        /// <summary>
        /// merge two streams into a single one
        /// in the merged stream, data is published out at its source's pace
        /// any source publish a value, that value will be published out from the merged stream
        /// </summary>
        static class TestMerge
        {
            private static void TestTwo()
            {
                IObservable<int> source1 = new int[] { 1, 2, 3, 4 }.ToObservableWithInterval(TimeSpan.FromSeconds(0.5));
                IObservable<int> source2 = new int[] { 100, 200, 300 }.ToObservableWithInterval(TimeSpan.FromSeconds(1));

                var merged = source1.Merge(source2).TimeInterval();

                // chekanote: the merged stream publish "OnCompleted" only when all its source completed
                merged.Subscribe(new TmIntervalPrinter<int>());

                Helper.Pause();
            }

            private static void TestMultiple()
            {
                var sources = new IObservable<int>[]
                                  {
                                      new int[] { 1, 2, 3, 4 }.ToObservableWithInterval(TimeSpan.FromSeconds(1)), 
                                      new int[] { 100, 200, 300 }.ToObservableWithInterval(TimeSpan.FromSeconds(0.6)),
                                      new int[] {1000,2000,3000,4000}.ToObservableWithInterval(TimeSpan.FromSeconds(0.75))
                                  };
                IObservable<int> merged = Observable.Merge(sources);

                // note: there is no need to synchronize this observer, because observer's execution are always serialized
                merged.Subscribe(new ConsolePrintObserver<int>());

                Helper.Pause();
            }

            public static void TestMain()
            {
                TestMultiple();
            }
        }

        /// <summary>
        /// connect whatever source which first response, ignore the other late ones
        /// </summary>
        static class TestAmb
        {
            public static void TestMain()
            {
                IObservable<int> source1 = new int[] { 1, 2, 3, 4 }.ToObservableWithInterval(TimeSpan.FromSeconds(2));
                IObservable<int> source2 = new int[] { 100, 200 }.ToObservableWithInterval(TimeSpan.FromSeconds(3));

                var ambed = source1.Amb(source2).TimeInterval();
                ambed.Subscribe(new TmIntervalPrinter<int>());

                Helper.Pause();
            }
        }

        [TestFixture]
        public sealed class TestConcat
        {
            private static void Check(Func<IObservable<int>, IObservable<int>, IObservable<int>> concater)
            {
                IObservable<int> source1 = new int[] { 1, 2, 3 }.ToObservableWithInterval(TimeSpan.FromSeconds(0.75));
                IObservable<int> source2 = new int[] { 100, 200, 300, 400 }.ToObservableWithInterval(TimeSpan.FromSeconds(1));

                IObservable<int> concated = concater(source1, source2);

                concated.Subscribe(new ConsolePrintObserver<int>());

                Helper.Pause();
            }

            private static void RunNoError()
            {
                // the second source is a cold one, because the second source begins publishing only after being subscribed
                // so all the items in the second source can appear in the concated stream
                Check((source1, source2) => source1.Concat(source2));

                // chekanote: the second source is a hot one, because the first and second publish items concurrently
                // so when the stream switch to the second stream, the items publish out before the switching
                // will not appear in the concated stream
                Check((source1, source2) =>
                {
                    var hotSource = source2.Publish();
                    hotSource.Connect();
                    return source1.Concat(hotSource);
                });
            }

            /// <summary>
            /// when the first stream has error, it won't switch to the second stream, but publish error 
            /// in the final concated stream
            /// IN SOME SITUATION, if you want to switch, you must use "Catch" other than "Concat" (but if no error, NO switch)
            /// </summary>
            [Test]
            public static void TestWhenFirstHasError()
            {
                IObservable<int> errorStream = Observable.Throw<int>(new NotSupportedException("test"));
                IObservable<int> concated = errorStream.Concat(Observable.Empty<int>());
                Assert.Throws<NotSupportedException>(() => concated.Subscribe(_ => { }));
            }

            /// <summary>
            /// concat multiple streams together by using class "Observable"'s static method
            /// </summary>
            [Test]
            public static void TestMultiples()
            {
                IObservable<int> stream1 = new[] { 1, 2 }.ToObservable();
                IObservable<int> stream2 = new[] { 3, 4 }.ToObservable();
                IObservable<int> stream3 = new[] { 7, 8 }.ToObservable();

                IObservable<int> concated = Observable.Concat(stream3, stream2, stream1);

                CollectionAssert.AreEqual(new[] { 7, 8, 3, 4, 1, 2 }, concated.ToEnumerable());
            }

            public static void TestMain()
            {
                // RunNoError();
                TestWhenFirstHasError();
            }
        }

        /*
         * stream1 ----0----1----2|       
         * stream2 --a--b--c--d--e--f|     
         * 
         * result  ----00--01-1--22-2|     
                       ab  cc d  de f    
         */
        static class TestCombineLast
        {
            // chekanote: pay attention to this method the fast source will lose its data before 
            // the slow one publish its first
            // before the slow one publish its first (that means the "last value" of the slow one is unavailable)
            // the fast one will not wait the slow one
            // the first item appear in the result stream is at the time when the slow one publish its first
            // chekanote: all data published by the fast one before that moment will be overwriten by the fast stream itself
            public static void TestMain()
            {
                IObservable<char> source1 = new char[] { 'a', 'b', 'c', 'd', 'e' }
                    .ToObservableWithInterval(TimeSpan.FromSeconds(0.25));
                IObservable<int> source2 = new int[] { 1, 2, 3 }.ToObservableWithInterval(TimeSpan.FromSeconds(1));

                var combined = source1.CombineLatest(source2, (x, y) => string.Format("{0}-{1}", x, y)).TimeInterval();
                combined.Subscribe(new TmIntervalPrinter<string>());

                Helper.Pause();
            }
        }

        #endregion

        // ********************************************************************* //
        #region "methods"

        private static void TestEqual()
        {
            IObservable<int> source1 = new int[] { 1, 2 }.ToObservableWithInterval(TimeSpan.FromSeconds(0.5));
            IObservable<int> source2 = new int[] { 1, 2 }.ToObservableWithInterval(TimeSpan.FromSeconds(1));
            IObservable<int> source3 = new int[] { 1 }.ToObservableWithInterval(TimeSpan.FromSeconds(0.5));

            IObservable<bool> equal = source1.SequenceEqual(source2);
            equal.Subscribe(flag => Console.WriteLine("source1 vs. source2: {0}", flag), () => Console.WriteLine("completed"));
            Helper.Pause();

            equal = source2.SequenceEqual(source3);
            equal.Subscribe(flag => Console.WriteLine("source2 vs. source3: {0}", flag), () => Console.WriteLine("completed"));
            Helper.Pause();
        }

        public static void TestMain()
        {
            // TestZip.TestMain();
            // TestMerge.TestMain();
            // TestAmb.TestMain();
            TestConcat.TestMain();
            // TestCombineLast.TestMain();
            // TestEqual();
        }

        #endregion
    }// TestCombination
}
