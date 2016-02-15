using System;
using System.Reactive.Linq;

namespace PlayRx
{
    static class TestWindow
    {
        // ********************************************************************** //
        #region "inner classes"

        sealed class WindowObserver : IObserver<IObservable<string>>
        {
            private int m_counter;

            public WindowObserver()
            {
                m_counter = 0;
            }

            /// <summary>
            /// window are generated at the start of the boundary
            /// </summary>
            public void OnNext(IObservable<string> value)
            {
                ++m_counter;
                Console.WriteLine("{0}-th window begins.", m_counter);

                int index = m_counter;
                int numMsgs = 0;
                value.Subscribe(
                    msg =>
                    {
                        ++numMsgs;
                        Console.WriteLine("\t[win-{0}]'s {1}-th: {2}", index, numMsgs, msg);
                    },
                    () => Console.WriteLine("\t[win-{0}] finished with total '{1}' messages.", index, numMsgs));
            }

            public void OnError(Exception error)
            {
                throw error;
            }

            public void OnCompleted()
            {
                Console.WriteLine("totally '{0}' windows finished", m_counter);
            }
        }

        #endregion

        // ********************************************************************** //
        #region "methods"

        private static void TestByCount_NonOverlap()
        {
            IObservable<IObservable<string>> source = Helper.MakeConsoleInputObservable().Window(3);
            source.Subscribe(new WindowObserver());
        }

        private static void TestByCount_Overlap()
        {
            IObservable<IObservable<string>> source = Helper.MakeConsoleInputObservable().Window(3, 2);
            source.Subscribe(new WindowObserver());
        }

        public static void TestMain()
        {
            TestByCount_NonOverlap();
            // TestByCount_Overlap();
        }

        #endregion
    }
}
