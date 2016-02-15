using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace PlayRx
{
    static class TestSelectMany
    {
        private static void Print(this IObservable<Tuple<int, int>> twoDimLoop)
        {
            twoDimLoop.Subscribe(pair => Console.WriteLine("{0}({1},{2})",
                new string('\t', pair.Item1),
                pair.Item1,
                pair.Item2));
        }

        /// <summary>
        /// chekanote: different from "TestInnerObservable", by returing Enumerable for each outer variable
        /// the inner loop is synchronous, it won't start next outer variable when iteration against 
        /// previous outer variable is completed. so the result will NEVER be interleaved.
        /// </summary>
        private static void TestInnerEnumerable()
        {
            IObservable<int> outloop = Observable.Range(1, 3);

            IObservable<Tuple<int, int>> twoDimLoop = outloop.SelectMany(
                outIndex => Enumerable.Range(outIndex * 100, 5),
                Tuple.Create);

            twoDimLoop.Print();
        }

        /// <summary>
        /// chekanote: from the result of below codes, we can see that iteration on different outer loop
        /// is interleaved. It will start the second outer variable, while the first outer variable is still looping
        /// looping different outer variables are in parallel
        /// since the result of SelectMany is "flattens the resulting observable sequences into one observable sequence", so when subscribed, observer's OnNext is still serialized
        /// the purpose of SelectMany is: when mapping each input element into an observable,
        /// it will introduce some parallelism, those projected child-observables will run in parallel
        /// </summary>
        private static void TestInnerObservable()
        {
            IObservable<int> outloop = Observable.Range(1, 3);

            IObservable<Tuple<int, int>> twoDimLoop = outloop.SelectMany(
                outIndex => Observable.Range(outIndex * 100, 5),
                Tuple.Create);

            // chekanote: pay attention here: the observer is blocked, so we didn't use Pause here
            // "Print" will exit only after all elements are processed
            // as I said above, SelectMany will introduce some parallelism, 
            // combined with the fact that "subscribe" is blocked
            // so I think, the parallelism here is introduced by using "CurrentThread" scheduler
            twoDimLoop.Print();
        }

        public static void TestMain()
        {
            TestInnerObservable();
            // TestInnerEnumerable();
        }
    }
}
