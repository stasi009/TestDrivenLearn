using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PlayRx
{
    static class TestDispose
    {
        private static void Consume(this IObserver<int> observer, int index)
        {
            Console.WriteLine("producing <{0}>,......", index);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            observer.OnNext(index);
        }

        private static void ConsumeSome(this IObservable<int> source, int expected)
        {
            source.Take(expected).Subscribe(
                num => Console.WriteLine("\t[{0}] consumed.", num),
                () => Console.WriteLine("consumption is totally completed."));
        }

        /// <summary>
        /// note: below test demonstrate "Take" will notify its observer to complete
        /// and unsubscribe from its source observable
        /// below two tests the generation of the source will be interrupted
        /// it won't generate "totol" items, but only "expected" items
        /// Take, like all the other operators, just subscribe on its source observable
        /// and keep the "IDisposable" returned, after enough items have been collected 
        /// it will call Dispose on that IDisposable to unsubscribe from its source 
        /// and then the generation will stoped (OTHER THAN the case that source will 
        /// generate all, and only some are delivered to the observer). THE ACTUAL CASE is
        /// the source will generate, and only generate, those expected ones and then 
        /// be interrupted and stop
        /// ********************* ONE EXECUTION RESULTS IS LIKE: 
        ///         producing <0>,......
        ///         [0] consumed.
        ///         producing <1>,......
        ///         [1] consumed.
        ///         consumption is totally completed.
        ///         !!! production is interrupted.
        ///         production completes.
        /// chekanote: pay attention that, "consumer" exits earlier than "producer"
        /// so it is the Take operator to notify the observer to complete 
        /// not the "completion" propagated from the original source
        /// </summary>
        private static void TestBoolDisposable(int total, int expected)
        {
            IObservable<int> source = Observable.Create<int>(observer =>
            {
                BooleanDisposable disposable = new BooleanDisposable();

                Task.Factory.StartNew(() =>
                {
                    for (int index = 0; index < total; index++)
                    {
                        if (disposable.IsDisposed)
                        {
                            Console.WriteLine("!!! production is interrupted.");
                            break;
                        }
                        else
                        {
                            observer.Consume(index);
                        }
                    }

                    observer.OnCompleted();
                    Console.WriteLine("production completes.");
                });// start task

                return disposable;
            });

            source.ConsumeSome(expected);
            Helper.Pause();
        }

        /// <summary>
        /// same as the previous "TestBoolDisposable", but the message that "production completes" NEVRE shows
        /// so it is much more clear that it is the Take operator notify the observer to complete 
        /// NOT the original source
        /// </summary>
        private static void TestCancelDisposable(int total, int expected)
        {
            IObservable<int> source = Observable.Create<int>(observer =>
            {
                CancellationDisposable disposable = new CancellationDisposable();

                Task.Factory.StartNew(() =>
                                          {
                                              for (int index = 0; index < total; index++)
                                              {
                                                  disposable.Token.ThrowIfCancellationRequested();
                                                  observer.Consume(index);
                                              }

                                              observer.OnCompleted();
                                              Console.WriteLine("[ NEVER DISPLAYED ] production completes.");
                                          }, disposable.Token);

                return disposable;
            });

            source.ConsumeSome(expected);
            Helper.Pause();
        }
        
        public static void TestMain()
        {
            // TestBoolDisposable(4, 2);
            TestCancelDisposable(4, 2);
        }// TestMain
    }
}
