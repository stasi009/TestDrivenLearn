using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using NUnit.Framework;

namespace PlayRx
{
    [TestFixture]
    sealed class TestError
    {
        private static void TestCatch_Simple()
        {
            var printer = new ConsolePrintObserver<int>();

            IObservable<int> errorSource = Observable.Throw<int>(new NotSupportedException("test"));
            errorSource.Subscribe(printer);

            // chekanote: here it catch the exception and return an empty observable, which is just like an empty catch block
            IObservable<int> catchAllSource = errorSource.Catch(Observable.Empty<int>());
            catchAllSource.Subscribe(printer);

            IObservable<int> catchRightSource = errorSource.Catch((NotSupportedException ex) => Observable.Empty<int>());
            catchRightSource.Subscribe(printer);

            // for those unexpected exception, it won't catch, and still fire "OnError" of the observer
            IObservable<int> catchNoneSource = errorSource.Catch((InvalidOperationException ex) => Observable.Empty<int>());
            catchNoneSource.Subscribe(printer);
        }

        /// <summary>
        /// by using "Catch", switch only occur when the first stream has an error, if the first stream completes normally
        /// it will never switch to the second stream
        /// </summary>
        [Test]
        public static void TestCatch_OnlySwitchOnError()
        {
            IObservable<int> stream1 = new[] { 1, 2 }.ToObservable();
            IObservable<int> stream2 = new[] { 3, 4 }.ToObservable();

            IObservable<int> errored = stream1.Concat(Observable.Throw<int>(new Exception("test"))).Catch(stream2);
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4 }, errored.ToEnumerable());

            IObservable<int> noerror = stream1.Catch(stream2);
            CollectionAssert.AreEqual(new[] { 1, 2 }, noerror.ToEnumerable());
        }

        [Test]
        public static void TestOnErrorResumeNext_AlwaysSwitch()
        {
            IObservable<int> stream1 = new[] { 1, 2 }.ToObservable();
            IObservable<int> stream2 = new[] { 3, 4 }.ToObservable();
            int[] expected = new[] {1, 2, 3, 4};

            IObservable<int> errored = stream1.Concat(Observable.Throw<int>(new Exception("test"))).OnErrorResumeNext(stream2);
            CollectionAssert.AreEqual(expected, errored.ToEnumerable());

            IObservable<int> noerror = stream1.OnErrorResumeNext(stream2);
            CollectionAssert.AreEqual(expected, noerror.ToEnumerable());
        }

        private static void TestRetry1()
        {
            IObservable<string> errorSource = Helper.MakeConsoleInputObservable();

            // chekanote: wether the callback attached to retrySource work or not
            // depends on whether there is observer attached to the original source
            // if there is observer subscribed to the original source, then observr for retrySource will not work
            // until the orginal source "OnError"
            // if there is no observer subscribed to the original source at first, the observer for retrySource
            // will always make effect
            errorSource.Subscribe(new ConsolePrintObserver<string>("original"));

            IObservable<string> retrySource = errorSource.Retry(3);
            retrySource.Subscribe(new ConsolePrintObserver<string>("retry", ConsoleColor.Green));
        }

        private static void TestRetry2()
        {
            IObservable<int> source = Observable.Create<int>(observer =>
                                                            {
                                                                foreach (int num in Enumerable.Range(1, 3))
                                                                {
                                                                    observer.OnNext(num);
                                                                }
                                                                observer.OnError(new NotSupportedException("test"));
                                                                return Disposable.Empty;
                                                            });

            try
            {
                // chekanote: everytime when exception is met, Retry will re-subscribe, which for cold observable,
                // that means restart the data production
                source.Retry(2).Subscribe(Console.WriteLine);
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("catch the exception: '{0}'", ex.Message);
            }
        }

        public static void TestMain()
        {
            TestCatch_Simple();
            // TestRetry1();
            // TestRetry2();
        }
    }
}
