using System;

namespace CSharpBasicTest
{
    /// <summary>
    /// this class is to demonstrate how to implement dispose for a sealed class
    /// </summary>
    sealed class ResourceWrapper : IDisposable
    {
        private bool m_disposed;

        ~ResourceWrapper()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (m_disposed)
                return;

            try
            {
                if (isDisposing)
                {
                    // to dispose that resource
                }
            }
            finally
            {
                m_disposed = true;
            }
        }
    }
}
