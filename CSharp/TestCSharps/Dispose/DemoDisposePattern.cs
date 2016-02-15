using System;

namespace CSharpBasicTest.Dispose
{
    static class DemoDisposePattern
    {
        // ******************************************************** //
        #region [ dispose sealed class ]

        private sealed class SealedDisposable : IDisposable
        {
            private bool m_disposed;

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            ~SealedDisposable()
            {
                Dispose(false);
            }

            private void Dispose(bool disposing)
            {
                if (!m_disposed)
                {
                    if (disposing)
                    {
                        // Free other state (managed objects).
                    }
                    // Free your own state (unmanaged objects).
                    // Set large fields to null.
                    m_disposed = true;
                }
            }
        }//SealedDisposable

        #endregion

        // ******************************************************** //
        #region [ dispose base and derived classes ]

        private class BaseDisposable : IDisposable
        {
            private bool m_disposed = false;

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!m_disposed)
                {
                    if (disposing)
                    {
                        // Free other state (managed objects).
                    }
                    // Free your own state (unmanaged objects).
                    // Set large fields to null.
                    m_disposed = true;
                }
            }

            ~BaseDisposable()
            {
                Dispose(false);
            }
        }

        private class DerivedDisposable : BaseDisposable
        {
            private bool m_disposed = false;

            protected override void Dispose(bool disposing)
            {
                if (!m_disposed)
                {
                    if (disposing)
                    {
                        // Release managed resources.
                    }
                    // Release unmanaged resources.
                    // Set large fields to null.
                    m_disposed = true;
                }
                base.Dispose(disposing);
            }
            // The derived class does not have a Finalize method
            // or a Dispose method without parameters because it inherits
            // them from the base class.
        }

        #endregion
    }
}
