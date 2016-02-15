using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace PlayRx
{
    static class TestObservableEx_Concurrent
    {
        private static void TestInterval()
        {
            int counter = 10;

            IObservable<long> intervalStream = Observable.Interval(TimeSpan.FromMilliseconds(500));

            IObservable<int> reverseCounters = Observable.Create<int>(
                observer => intervalStream.Subscribe(value =>
                {
                    --counter;

                    if (counter > 0)
                        observer.OnNext(counter);
                    else
                        observer.OnCompleted();
                }));

            reverseCounters.Subscribe(Console.WriteLine, () => Console.WriteLine("Done"));

            Helper.Pause();
        }

        public static void TestMain()
        {
            TestInterval();
        }
    }
}
