using System;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;

namespace PlayRx.ServerSide
{
    static class TestCopyFile
    {
        public static IObservable<byte[]> MinimumBuffer(this IObservable<byte[]> input, int minimumBufSize)
        {
            return Observable.Create<byte[]>(observer =>
            {
                MemoryStream memStream = new MemoryStream();

                return input.Subscribe(
                    bytes =>
                    {
                        memStream.Write(bytes, 0, bytes.Length);
                        if (memStream.Length >= minimumBufSize)
                        {
                            Debug.Assert(memStream.Position == memStream.Length);
                            observer.OnNext(memStream.ToArray());

                            memStream.SetLength(0);// truncate the stream
                            memStream.Position = 0;
                        }
                    },
                    observer.OnError,
                    () =>
                    {
                        if (memStream.Length > 0)
                            observer.OnNext(memStream.ToArray());
                        observer.OnCompleted();
                    }
                    );
            });
        }

        private static IObservable<byte[]> SimpleObservableBytes(string fileName, int bufferSize)
        {
            Stream inputStream = new FileStream(fileName, FileMode.Open, FileAccess.Read,
                                                FileShare.Read, bufferSize, true);
            return new ReadStreamObservable(inputStream, bufferSize).Finally(() =>
            {
                Console.WriteLine("##### input stream is closed.");
                inputStream.Close();
            });
        }

        private static void CheckCopy(string oriFileName, int bufferSize, Func<string, int, IObservable<byte[]>> inputSourceFactory)
        {
            // ----------------- initialize input
            IObservable<byte[]> inputSource = inputSourceFactory(oriFileName, bufferSize);

            // ----------------- prepare output
            string cpyFileName = "copy_" + oriFileName;
            Stream outputStream = new FileStream(cpyFileName, FileMode.Create, FileAccess.Write, FileShare.None,
                                                 bufferSize, true);
            Func<byte[], int, int, IObservable<Unit>> funcAsyncWrite =
                Observable.FromAsyncPattern<byte[], int, int>(outputStream.BeginWrite, outputStream.EndWrite);

            // here we use "where false", because we don't care about the signal that "write completes"
            // we only care about the final signal that "all async-write have completes"
            IObservable<Unit> outputSource = (from bytes in inputSource
                                              from writeResult in funcAsyncWrite(bytes, 0, bytes.Length)
                                              // where false
                                              select writeResult)
                                             .Finally(() =>
                                                          {
                                                              outputStream.Close();
                                                              Console.WriteLine("##### output stream is closed.");
                                                          });

            // ----------------- begin copying (remove the "where false" if you want to see the progress)
            int index = 0;
            using (outputSource.Subscribe(_ =>
            {
                ++index;
                Console.WriteLine("{0}-writing finished.", index);
            },
            () => Console.WriteLine("!!! COPY FINISHED !!!")))
            {
                Helper.Pause();
            }
        }

        private static void CheckBufferedObservableBytes(string oriFileName)
        {
            int readSize = 1024;
            int miniBufSize = readSize * 2;
            CheckCopy(oriFileName, readSize, (fileName, size) => SimpleObservableBytes(fileName, size).MinimumBuffer(miniBufSize));
        }

        public static void TestMain()
        {
            // CheckCopy("Sample1344.PmuCapture", 1024, SimpleObservableBytes);
            CheckBufferedObservableBytes("Sample1344.PmuCapture");
        }
    }
}
