using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace PlayRx
{
    static class TestToAsync
    {
        private static int LongParsing(string strNumber)
        {
            Console.WriteLine("begin processing '{0}',......", strNumber);

            int sleep = int.Parse(strNumber);
            Thread.Sleep(TimeSpan.FromMilliseconds(sleep));

            Console.WriteLine("\tfinish processing '{0}'", strNumber);
            return sleep;
        }

        private static IObservable<int> ObservableLongParsing(string strNumber)
        {
            Func<string, IObservable<int>> factory = new Func<string, int>(LongParsing).ToAsync();
            return factory(strNumber);
        }

        private static void CheckToAsync_Single()
        {
            IObservable<int> source = ObservableLongParsing("1000");
            source.Subscribe(num => Console.WriteLine("parsed result={0}", num),
                             () => Console.WriteLine("done !!!"));

            // chekanote: ToAsync will import concurrency, so we must block the main thread
            Helper.Pause();
        }

        private static void CheckToAsync_InParallel()
        {
            string[] strNumbers = { "1000", "1500", "2000" };
            IEnumerable<IObservable<int>> streams = from str in strNumbers
                                                    select ObservableLongParsing(str);

            IObservable<int> singleStream = streams.Merge();

            singleStream.Subscribe(
                num => Console.WriteLine("[{0}] published from thread<{1}>", num, Thread.CurrentThread.ManagedThreadId),
                () => Console.WriteLine("completion published from thread<{0}>", Thread.CurrentThread.ManagedThreadId));

            Helper.Pause();
        }

        public static void TestMain()
        {
            // CheckToAsync_Single();
            CheckToAsync_InParallel();
        }
    }
}
