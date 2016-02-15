using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace PlayRx
{
    sealed class ConsolePrintObserver<T> : IObserver<T>
    {
        private volatile int m_counter;
        private readonly ConsoleColor m_defaultColor;
        private readonly string m_name;
        private readonly string m_dent;

        public ConsolePrintObserver(string name = "", ConsoleColor defaultColor = ConsoleColor.Cyan, int dent = 1)
        {
            m_name = name;
            m_defaultColor = defaultColor;
            m_counter = 0;
            m_dent = new string('\t', dent);
        }

        public void OnNext(T value)
        {
            ++m_counter;

            Console.ForegroundColor = m_defaultColor;
            Console.WriteLine("{0}[{1}{2}] {3}", m_dent, m_name, m_counter, value);
        }

        public void OnError(Exception error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}{1} Error: {2}", m_dent, m_name, error.Message);
            Console.ResetColor();
        }

        public void OnCompleted()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0}{1} Completed !", m_dent, m_name);
            Console.ResetColor();
        }
    }

    sealed class RecordObserver : IObserver<int>
    {
        public int? LastValue { get; private set; }
        public bool IsCompleted { get; private set; }
        public Exception Error { get; private set; }
        public int NumOfNextInvoke { get; private set; }

        public RecordObserver()
        {
            this.LastValue = null;
            this.IsCompleted = false;
            this.Error = null;
            this.NumOfNextInvoke = 0;
        }

        #region "implement IObserver"

        public void OnNext(int value)
        {
            this.LastValue = value;
            ++this.NumOfNextInvoke;
        }

        public void OnError(Exception error)
        {
            this.Error = error;
        }

        public void OnCompleted()
        {
            this.IsCompleted = true;
        }

        #endregion
    }

    static class Helper
    {
        public static void Pause(string hint = null)
        {
            Console.WriteLine("Press ENTER to {0}, ......", string.IsNullOrEmpty(hint) ? "continue" : hint);
            Console.ReadLine();
        }

        public static void BlockResponse<T>(T value)
        {
            Console.WriteLine("Blocked at <{0}>!!! Press ENTER to unblock, ......", value);
            Console.ReadLine();
            Console.WriteLine("<{0}> is processed.\n", value);
        }

        public static IEnumerable<int> DelayBlockEnumerable(int start, int count, TimeSpan interval)
        {
            IEnumerable<int> basic = Enumerable.Range(start, count);
            foreach (int num in basic)
            {
                Thread.Sleep(interval);
                yield return num;
            }
        }

        public static IObservable<string> MakeConsoleInputObservable()
        {
            return Observable.Create<string>(observer =>
            {
                while (true)
                {
                    string input = Console.ReadLine();
                    if (input.Equals("exit"))
                    {
                        observer.OnCompleted();
                        break;
                    }
                    else if (input.Equals("error"))
                    {
                        observer.OnError(new ArgumentException("'error' input"));
                        break;
                    }
                    else
                        observer.OnNext(input);
                }
                return () => Console.WriteLine("UnSubscribed.");
            });
        }

        public static IObservable<T> ToObservableWithInterval<T>(this IList<T> array, TimeSpan interval)
        {
            return Observable.Generate(0,
                                       index => index < array.Count,
                                       index => index + 1,
                                       index => array[index],
                                       index => interval);
        }

        public static string ToMinSecString(this DateTime dt)
        {
            return string.Format("{0}:{1}", dt.Minute, dt.Second);
        }
    }// Helper
}
