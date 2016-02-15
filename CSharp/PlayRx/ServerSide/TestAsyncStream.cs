using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace PlayRx.ServerSide
{
    static class TestAsyncStream
    {
        private static Stream MakeReadableStream(IEnumerable<int> numbers)
        {
            Stream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            foreach (int number in numbers)
            {
                writer.Write(number);
            }

            writer.Flush();
            stream.Position = 0;// return back to start for reading

            return stream;
        }

        private static void ReadingSample()
        {
            const int original = 88;
            Stream stream = MakeReadableStream(Enumerable.Repeat(original, 1));

            byte[] recvBuffer = new byte[4];
            Func<byte[], int, int, IObservable<int>> funcAsyncRead = Observable.FromAsyncPattern<byte[], int, int, int>(
                stream.BeginRead,
                stream.EndRead);

            IObservable<int> source = funcAsyncRead(recvBuffer, 0, recvBuffer.Length);
            source.ForEach(num => Console.WriteLine("{0} bytes read", num));// block until reading totally completes
            Console.WriteLine("reading completes");
            Debug.Assert(4 == stream.Position);

            int parseFromRecv = BitConverter.ToInt32(recvBuffer, 0);
            Debug.Assert(parseFromRecv == original);

            // !!!!!!!! chekanote: below codes shows that the Observable get from FromAsyncPattern
            // !!!!!!!! is a HOT and STATEFUL one
            // !!!!!!!! HOT: because for the second subscriber, it didn't read again( because the stream has already reached end, if read again, it must throw exception. however, no exception is thrown here)
            // !!!!!!!! chekanote: also because 'FromAsyncPattern' is based on AsyncSubject
            // !!!!!!!! STATEFUL: the returned Iobservable will remeber its state for late observer
            Helper.Pause("next subscriber");
            source.Subscribe(new ConsolePrintObserver<int>());
        }

        private static void ReadOnce_RepeatMultipleTimes()
        {
            const int COUNT = 3;
            Stream stream = MakeReadableStream(Enumerable.Range(1, COUNT));

            byte[] recvBuffer = new byte[4];

            for (int index = 0; index < COUNT; index++)
            {
                Console.WriteLine("**************** {0}-th reading, ......", index + 1);

                Func<byte[], int, int, IObservable<int>> funcAsyncRead = Observable.FromAsyncPattern<byte[], int, int, int>
                    (stream.BeginRead, stream.EndRead);

                // chekanote: 'funcAsyncRead' returns an AsyncSubject
                // so the async-reading only occur once, async-reading completes even before being subscribed
                // multiple "Repeat" will not incur multiple reading
                // they will just process the same results multiple times
                IObservable<int> source = funcAsyncRead(recvBuffer, 0, recvBuffer.Length).Repeat(COUNT);
                source.ForEach(num =>
                                   {
                                       int parsed = BitConverter.ToInt32(recvBuffer, 0);
                                       Console.WriteLine("\t{0} bytes read, parsed value={1}", num, parsed);
                                   });
                Console.WriteLine("{0}-th reading completes.", index + 1);
                Helper.Pause("read next");
            }// for
        }

        private static void RepeatDeferReading()
        {
            int COUNT = 6;
            Stream stream = MakeReadableStream(Enumerable.Range(2, COUNT));

            Func<byte[], int, int, IObservable<int>> funcAsyncRead = Observable.FromAsyncPattern<byte[], int, int, int>(stream.BeginRead, stream.EndRead);

            byte[] recvBuffer = new byte[4];
            IObservable<int> source = Observable.Defer(() => funcAsyncRead(recvBuffer, 0, recvBuffer.Length)).Repeat(COUNT);

            // when meeting the end, "reading" returns 0, other than throwing an exception
            // so I have to use "TakeWhile" to terminate the reading based on the reading result
            source.TakeWhile(num => num > 0)
                .Subscribe(num =>
                {
                    Debug.Assert(num == 4, "impossible for partial reading");
                    int parsed = BitConverter.ToInt32(recvBuffer, 0);
                    Console.WriteLine("{0} is read out.", parsed);
                },
                err => Console.WriteLine(err.Message),
                () => Console.WriteLine("!!!!! completes !!!!!"));

            Helper.Pause();
        }

        private static void CheckFromApm()
        {
            Stream stream = MakeReadableStream(Enumerable.Range(8, 10));

            IObservable<byte[]> source = new ReadStreamObservable(stream, 4);

            using (source.Subscribe(bytes =>
                                 {
                                     Debug.Assert(bytes.Length == 4, "impossible for partial reading");

                                     int parsed = BitConverter.ToInt32(bytes, 0);
                                     Thread.Sleep(TimeSpan.FromSeconds(0.5));// simulate long-time processing

                                     Console.WriteLine("{0} is read out.", parsed);
                                 }, () => Console.WriteLine("!!! reading completed !!!")))
            {
                Helper.Pause();
            }
        }

        public static void TestMain()
        {
            // ReadingSample();
            ReadOnce_RepeatMultipleTimes();
            // RepeatDeferReading();
            // CheckFromApm();
        }// TestMain
    }
}
