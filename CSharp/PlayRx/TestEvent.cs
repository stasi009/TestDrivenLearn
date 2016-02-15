using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;

namespace PlayRx
{
    static class TestEvent
    {
        private sealed class Publisher
        {
            public event Action<string> NonClassicEvent;

            private int m_value = 0;
            public event EventHandler ValueChanged;

            public int Value { get { return m_value; } }
            public void Increase()
            {
                ++m_value;
                if (ValueChanged != null)
                    ValueChanged(this, EventArgs.Empty);
            }

            public void Fire(string msg)
            {
                if (NonClassicEvent != null)
                    NonClassicEvent(msg);
            }
        }

        /// <summary>
        /// 1. before subscribing, the event will always be null
        /// 2. no "completion" will be fired
        /// </summary>
        private static void TestFromEvent()
        {
            Publisher evtSource = new Publisher();

            IObservable<string> stream = Observable.FromEvent<string>(
                handler => evtSource.NonClassicEvent += handler,
                handler => evtSource.NonClassicEvent -= handler);

            // -------------- no subscribers yet, so this message will be forgotten
            evtSource.Fire("!!! this message will never be displayed.");

            stream.Subscribe(
                msg => Console.WriteLine("'{0}' received", msg),
                _ => Debug.Fail("done !!! (WILL ALSO NEVER SHOW UP)"));

            foreach (string msg in new[] { "hello", "wsu" })
            {
                evtSource.Fire(msg);
            }
        }

        private static void TestFromEventPattern()
        {
            Publisher publisher = new Publisher();

            var stream = Observable.FromEventPattern(
                handler => publisher.ValueChanged += handler,
                handler => publisher.ValueChanged -= handler);

            publisher.Increase();// nothing will be fired, because no subscribers yet

            stream.Subscribe(
                evtpattern =>
                {
                    Publisher src = (Publisher)evtpattern.Sender;
                    Console.WriteLine("current value={0}", src.Value);
                },
                _ => Debug.Fail("never completes"));

            for (int index = 0; index < 3; index++)
            {
                publisher.Increase();
            }
        }

        public static void TestMain()
        {
            // TestFromEvent();
            TestFromEventPattern();
        }
    }
}
