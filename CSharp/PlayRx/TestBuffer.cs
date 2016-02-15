using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace PlayRx
{
    static class TestBuffer
    {
        sealed class BufferObserver<T> : IObserver<IList<T>>
        {
            private int m_counter = 0;

            public void OnNext(IList<T> value)
            {
                ++m_counter;
                Console.WriteLine("[buffer-{0}] ends", m_counter);

                for (int index = 0; index < value.Count; index++)
                {
                    Console.WriteLine("\t[buffer-{0}]'s {1}-th: {2}", m_counter, index, value[index]);
                }
            }

            public void OnError(Exception error)
            {
                throw error;
            }

            public void OnCompleted()
            {
                Console.WriteLine("completed with {0} buffers in total", m_counter);
            }
        }

        private static void TestByCount_NonOverlap()
        {
            IObservable<IList<string>> source = Helper.MakeConsoleInputObservable().Buffer(3);
            source.Subscribe(new BufferObserver<string>());
        }

        private static void TestByCount_Overlap()
        {
            IObservable<IList<string>> source = Helper.MakeConsoleInputObservable().Buffer(4, 2);
            source.Subscribe(new BufferObserver<string>());
        }

        private static void TestByTimespan()
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(0.5))
                .Take(20)
                .Buffer(TimeSpan.FromSeconds(2));
            source.Subscribe(new BufferObserver<long>());

            Helper.Pause();
        }

        public static void TestMain()
        {
            // TestByCount_NonOverlap();
            TestByCount_Overlap();
            // TestByTimespan();
        }
    }
}
