using System;
using System.Collections.Concurrent;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace PlayRx
{
    static class TestAgent
    {
        #region "classes"
        sealed class Str2IntAgent : ISubject<string, int>
        {
            private IObserver<int> m_observer;

            #region "observer for string"
            public void OnNext(string value)
            {
                m_observer.OnNext(value.Length);
            }

            public void OnError(Exception error)
            {
                m_observer.OnError(error);
            }

            public void OnCompleted()
            {
                m_observer.OnCompleted();
            }
            #endregion

            #region "observable for int"

            public IDisposable Subscribe(IObserver<int> observer)
            {
                m_observer = observer;
                return Disposable.Empty;
            }

            #endregion
        }

        sealed class MergeAgent<T> : ISubject<T>
        {
            private readonly BlockingCollection<T> m_queue;
            private int m_completeCounter;
            private readonly int m_completeThreshold;

            public MergeAgent(int threshold)
            {
                m_queue = new BlockingCollection<T>();
                m_completeThreshold = threshold;
                m_completeCounter = 0;
            }

            #region 'observer'
            public void OnNext(T value)
            {
                if (!m_queue.IsAddingCompleted)
                    m_queue.Add(value);
            }

            public void OnError(Exception error)
            {
                throw error;
            }

            public void OnCompleted()
            {
                if (Interlocked.Increment(ref m_completeCounter) >= m_completeThreshold)
                    m_queue.CompleteAdding();
            }
            #endregion

            #region 'observable'

            public IDisposable Subscribe(IObserver<T> observer)
            {
                foreach (T value in m_queue.GetConsumingEnumerable())
                {
                    observer.OnNext(value);
                }
                observer.OnCompleted();

                return Disposable.Empty;
            }

            #endregion
        }

        #endregion

        #region "methods"

        private static void CheckStr2IntAgent()
        {
            // this agent, as a observable, is a hot one
            // being subscribed won't start the observable
            // so the order is very important, we must first subscribe the observer to the agent here
            Str2IntAgent agent = new Str2IntAgent();
            agent.Subscribe(new ConsolePrintObserver<int>());

            IObservable<string> source = new string[] { "Hello", "WSU", "Stasi", "KGB", "GRU", "Cheka", "MSS" }
                .ToObservableWithInterval(TimeSpan.FromSeconds(1));
            source.Subscribe(agent);

            Helper.Pause();
        }

        private static IObservable<string> MakeIntervaledStrStream(string prefix, int total, int dent, TimeSpan interval)
        {
            return (from index in Observable.Interval(interval)
                    select string.Format("{0}{1}-{2}", new string('\t', dent), prefix, index)).Take(total);
        }

        private static void CheckMergeAgent()
        {
            IObservable<string>[] streams = 
            {
                MakeIntervaledStrStream("hello",5,0,TimeSpan.FromSeconds(1)),
                MakeIntervaledStrStream("stasi",12,1,TimeSpan.FromSeconds(0.5)),
                MakeIntervaledStrStream("wsu",15,2,TimeSpan.FromSeconds(0.3))
            };

            MergeAgent<string> agent = new MergeAgent<string>(streams.Length);
            foreach (IObservable<string> stream in streams)
            {
                stream.Subscribe(agent);
            }

            agent.Subscribe(Console.WriteLine,
                            () => Console.WriteLine("!!! finished !!!"));
        }

        public static void TestMain()
        {
            CheckMergeAgent();
        }

        #endregion
    }
}
