using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

using NUnit.Framework;

namespace PlayRx
{
    /// <summary>
    /// test the non-concurrency involved extension method of "Observable"
    /// </summary>
    [TestFixture]
    sealed class TestObservableEx_NoConcurrent
    {
        #region "internal helper class"
        sealed class Sink : IObserver<int>
        {
            private int m_currentValue = 0;
            private bool m_valueSet = false;
            private bool m_completed = false;
            private Exception m_error;

            public void Reset()
            {
                m_currentValue = 0;
                m_valueSet = false;
                m_completed = false;
            }

            public bool IsCompleted { get { return m_completed; } }

            public int? CurrentValue { get { return m_valueSet ? m_currentValue : (int?)null; } }

            public Exception Error { get { return m_error; } }

            #region "implement IObserver"
            public void OnNext(int value)
            {
                m_currentValue = value;
                m_valueSet = true;
            }

            public void OnError(Exception error)
            {
                m_error = error;
            }

            public void OnCompleted()
            {
                m_completed = true;
            }
            #endregion
        }
        #endregion

        [Test]
        public static void TestEmpty()
        {
            IObservable<int> emptyStream = Observable.Empty<int>();

            Sink sink = new Sink();
            emptyStream.Subscribe(sink);

            Assert.IsFalse(sink.CurrentValue.HasValue);
            Assert.IsTrue(sink.IsCompleted);
        }

        [Test]
        public static void TestReturn()
        {
            IObservable<int> singleStream = Observable.Return(100);

            Sink sink = new Sink();
            singleStream.Subscribe(sink);

            Assert.IsTrue(sink.CurrentValue.HasValue);
            Assert.AreEqual(100, sink.CurrentValue);
            Assert.IsTrue(sink.IsCompleted);

            // --------------------- test the cold feature, run it once again
            sink.Reset();
            Assert.IsFalse(sink.CurrentValue.HasValue);
            Assert.IsFalse(sink.IsCompleted);

            singleStream.Subscribe(sink);
            Assert.AreEqual(100, sink.CurrentValue);
            Assert.IsTrue(sink.IsCompleted);
        }

        public static void TestNever()
        {
            // as a infinite stream, maybe "Never" will run asynchronously
            IObservable<int> infiniteStream = Observable.Never<int>();
            infiniteStream.Subscribe(Console.WriteLine, () => Console.WriteLine("done."));

            Helper.Pause();
        }

        /// <remark>
        /// this test demonstrate that when constructing an IObservable
        /// it actually constructs a state machine, and the parameters has been fixed into the constructor when constructing
        /// chekanote: NOT captured variable
        /// so when outside variables are changed, it won't affect the state machine when it runs again
        /// </remark>
        [Test]
        public static void TestParamFixedWhenConstruct()
        {
            int num = 101;
            IObservable<int> singleStream = Observable.Return(num);

            Sink sink1 = new Sink();
            singleStream.Subscribe(sink1);

            Assert.AreEqual(101, sink1.CurrentValue);

            // ======================== change the outside variable, it won't affect the cold observable already built
            num = 88;

            Sink sink2 = new Sink();
            singleStream.Subscribe(sink2);

            Assert.AreNotEqual(88, sink2.CurrentValue);
            Assert.AreEqual(101, sink2.CurrentValue);
        }

        [Test]
        public static void TestThrow()
        {
            IObservable<int> errorStream = Observable.Throw<int>(new InvalidOperationException());

            Sink sink = new Sink();
            errorStream.Subscribe(sink);

            Assert.IsTrue(sink.Error is InvalidOperationException);
        }

        private static void TestCreate()
        {
            int numSubscribers = 0;

            // chekanote: the returned observable is a cold one
            // the action body passed in will be executed everytime "Subscribe" is invoked
            // generated observable is cold, everytime subscribed, it will run from fresh start to end
            IObservable<int> stream = Observable.Create<int>(observer =>
            {
                ++numSubscribers;
                observer.OnNext(numSubscribers);

                // chekanote: no "OnComplete" here, so the subscription will not be disposed automatically
                // but have to be disposed manually

                // return action which will be executed when the subscription is disposed
                return () =>
                           {
                               --numSubscribers;
                               Console.WriteLine("unsubscribed,current number={0}", numSubscribers);
                           };
            });

            Action<int> onnextCallback = num => Console.WriteLine("currently has {0} subscription.", num);
            Action oncompleteCallback = () => Console.WriteLine("finished");

            IDisposable subscription1 = stream.Subscribe(onnextCallback, oncompleteCallback);
            IDisposable subscription2 = stream.Subscribe(onnextCallback, oncompleteCallback);

            subscription1.Dispose();
            subscription2.Dispose();
        }

        private static void TestCreateAutoDispose()
        {
            IObservable<int> stream = Observable.Create<int>(observer =>
            {
                observer.OnNext(88);
                observer.OnCompleted();// auto-dispose

                return () => Console.WriteLine("subscription disposed");
            });

            // chekanote: no need to dispose the subscription manually, it will automatically disposed when the stream completed
            stream.Subscribe(Console.WriteLine, () => Console.WriteLine("1st done"));

            // everytime it is subscribed, the action body will be executed once again, so it is a cold observable
            stream.Subscribe(Console.WriteLine, () => Console.WriteLine("2nd done"));
        }

        private static void TestRange()
        {
            int start = 1;
            int total = 9;
            IObservable<int> stream = Observable.Range(start, total);

            // chekanote: useless update, because parameter has already been fixed when constructing the observable
            start = 5;
            total = 100000;

            stream.Subscribe(num => Console.Write("{0} ", num), () => Console.WriteLine());

            Console.WriteLine("Block until done.");
        }

        public static void TestMain()
        {
            // TestNever();
            // TestCreate();
            // TestCreateAutoDispose();
            TestRange();
        }
    }
}
