using System;
using System.Reactive.Subjects;
using NUnit.Framework;

namespace PlayRx
{
    [TestFixture]
    sealed class TestSubjects2
    {
        #region 'Subject'

        [Test]
        public static void Subject_AfterCompletion()
        {
            ISubject<int> subject = new Subject<int>();

            RecordObserver earlyRecorder = new RecordObserver();
            subject.Subscribe(earlyRecorder);

            subject.OnNext(1);
            subject.OnNext(10);
            subject.OnCompleted();
            subject.OnNext(3);// publication after completion, total useless

            Assert.AreEqual(10, earlyRecorder.LastValue);
            Assert.AreEqual(2, earlyRecorder.NumOfNextInvoke);
            Assert.IsTrue(earlyRecorder.IsCompleted);

            // --------------- subscription after completion, 
            // --------------- no value will published out, no "OnNext" will be invoked
            // --------------- chekanote: but "OnCompleted" will be invoked once
            RecordObserver lateRecorder = new RecordObserver();
            subject.Subscribe(lateRecorder);

            Assert.IsFalse(lateRecorder.LastValue.HasValue);
            Assert.AreEqual(0, lateRecorder.NumOfNextInvoke);
            Assert.IsTrue(lateRecorder.IsCompleted);
        }

        #endregion

        #region 'ReplaySubject'

        [Test]
        public static void ReplaySubject_RememberAll()
        {
            ISubject<int> subject = new ReplaySubject<int>();

            subject.OnNext(1);
            subject.OnNext(88);
            subject.OnNext(999);

            var recorder = new RecordObserver();
            subject.Subscribe(recorder);

            // all values before the subscription will be remembered and published out as soon as new Observer are subscribed
            Assert.AreEqual(3, recorder.NumOfNextInvoke);
            Assert.AreEqual(999, recorder.LastValue);
            Assert.IsFalse(recorder.IsCompleted);
        }

        [Test]
        public static void ReplaySubject_AfterCompletion()
        {
            ISubject<int> subject = new ReplaySubject<int>();

            subject.OnNext(1);
            subject.OnNext(88);
            subject.OnNext(999);
            subject.OnCompleted();

            var recorder = new RecordObserver();
            subject.Subscribe(recorder);

            Assert.AreEqual(3, recorder.NumOfNextInvoke);
            Assert.IsTrue(recorder.LastValue.HasValue);
            Assert.AreEqual(999, recorder.LastValue);
            Assert.IsTrue(recorder.IsCompleted);
        }

        #endregion

        #region 'BehaviorSubject'

        [Test]
        public static void BehaviorSubject_DefaultValue()
        {
            var recorder = new RecordObserver();

            ISubject<int> subject = new BehaviorSubject<int>(101);
            subject.Subscribe(recorder);

            Assert.AreEqual(1, recorder.NumOfNextInvoke);
            Assert.AreEqual(101, recorder.LastValue);
            Assert.IsFalse(recorder.IsCompleted);
        }

        [Test]
        public static void BehaviorSubject_Overwrite()
        {
            var recorder = new RecordObserver();

            ISubject<int> subject = new BehaviorSubject<int>(1);
            subject.OnNext(2);
            subject.OnNext(88);
            subject.Subscribe(recorder);

            // different from ReplaySubject, only the last value is remembered
            // previous values are forgotten, so they won't fire "OnNext"
            Assert.AreEqual(1, recorder.NumOfNextInvoke);
            Assert.AreEqual(88, recorder.LastValue);
            Assert.IsFalse(recorder.IsCompleted);
        }

        [Test]
        public static void BehaviorSubject_AfterCompletion()
        {
            var recorder = new RecordObserver();

            ISubject<int> subject = new BehaviorSubject<int>(1);
            subject.OnNext(11);
            subject.OnNext(999);
            subject.OnCompleted();

            subject.Subscribe(recorder);

            // subscribing after completion, no "OnNext" will be fired, but "OnCompletion" will be fired
            Assert.AreEqual(0, recorder.NumOfNextInvoke);
            Assert.IsFalse(recorder.LastValue.HasValue);
            Assert.IsTrue(recorder.IsCompleted);
        }

        #endregion

        #region 'AsyncSubject'

        /// <summary>
        /// AsyncSubject only publish out value when completed
        /// before that no data is published out from AsyncSubject
        /// </summary>
        [Test]
        public static void AsyncSubject_OnlyPubWhenCompleted()
        {
            var recorder = new RecordObserver();

            ISubject<int> subject = new AsyncSubject<int>();
            subject.Subscribe(recorder);

            subject.OnNext(1);
            subject.OnNext(2);
            Assert.IsFalse(recorder.LastValue.HasValue);
            Assert.AreEqual(0, recorder.NumOfNextInvoke);

            subject.OnCompleted();
            Assert.IsTrue(recorder.LastValue.HasValue);
            Assert.AreEqual(2, recorder.LastValue);
            Assert.IsTrue(recorder.IsCompleted);
            Assert.AreEqual(1, recorder.NumOfNextInvoke);
        }

        [Test]
        public static void AsyncSubject_AfterCompletion()
        {
            var earlyRecorder = new RecordObserver();

            ISubject<int> subject = new AsyncSubject<int>();
            subject.Subscribe(earlyRecorder);

            subject.OnNext(1);
            subject.OnNext(88);
            subject.OnCompleted();

            Assert.AreEqual(88, earlyRecorder.LastValue);
            Assert.AreEqual(1, earlyRecorder.NumOfNextInvoke);

            // AsyncSubject is stateful, even subscribing after completion, the observer can also get data
            var lateRecorder = new RecordObserver();
            subject.Subscribe(lateRecorder);

            Assert.IsTrue(lateRecorder.IsCompleted);
            Assert.AreEqual(88, lateRecorder.LastValue);
            Assert.AreEqual(1, lateRecorder.NumOfNextInvoke);
        }

        #endregion
    }
}
