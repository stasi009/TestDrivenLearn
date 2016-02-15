using System;
using System.IO;
using System.Reactive.Linq;

namespace PlayRx.ServerSide
{
    sealed class ReadStreamObservable : IObservable<byte[]>
    {
        // *************************************************************** //
        #region [ member fields ]

        private readonly Stream m_stream;
        private readonly byte[] m_recvBuffer;

        #endregion

        // *************************************************************** //
        #region [ constructor ]

        public ReadStreamObservable(Stream stream, int bufferSize)
        {
            m_stream = stream;
            m_recvBuffer = new byte[bufferSize];
        }

        #endregion

        // *************************************************************** //
        #region [ implement IObservable ]

        public IDisposable Subscribe(IObserver<byte[]> dataConsumer)
        {
            IObservable<byte[]> source = Observable.Create<byte[]>(observer =>
            {
                StartRead(observer);
                return m_stream;
            });
            return source.Subscribe(dataConsumer);
        }

        #endregion

        // *************************************************************** //
        #region [ private helpers ]

        private void StartRead(IObserver<byte[]> observer)
        {
            m_stream.BeginRead(m_recvBuffer, 0, m_recvBuffer.Length, OnReadCompleted, observer);
        }

        private void OnReadCompleted(IAsyncResult asyncResult)
        {
            IObserver<byte[]> observer = (IObserver<byte[]>)asyncResult.AsyncState;
            try
            {
                int readed = m_stream.EndRead(asyncResult);

                if (readed < 0)
                    observer.OnError(new Exception("Read returns negative"));
                else if (readed == 0)
                    observer.OnCompleted();
                else
                {
                    byte[] outputBuffer = new byte[readed];
                    Buffer.BlockCopy(m_recvBuffer, 0, outputBuffer, 0, readed);
                    observer.OnNext(outputBuffer);

                    StartRead(observer);
                }
            }
            catch (Exception ex)
            {
                observer.OnError(ex);
            }
        }

        #endregion
    }
}
