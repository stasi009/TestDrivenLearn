using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace AsyncWPF
{
    sealed class BackgrdWorkerPresenter : CounterPresenterBase
    {
        private BackgroundWorker m_backgrdWorker;

        public BackgrdWorkerPresenter(int total, int intervalInMs, IView view)
            : base(total, intervalInMs, view)
        {
            // note: this background worker will be created on UI thread
            // (because this constructor will be called in UI's constructor)
            m_backgrdWorker = new BackgroundWorker { WorkerReportsProgress = true };
            m_backgrdWorker.DoWork += new DoWorkEventHandler(OnDoWork);
            m_backgrdWorker.ProgressChanged += new ProgressChangedEventHandler(OnProgressChanged);
            m_backgrdWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnCompleted);
        }

        public override void Start()
        {
            m_view.OnStart();
            m_backgrdWorker.RunWorkerAsync();
        }

        #region "event handler"

        // note: run in UI thread (more precisely, it will run in the same thread 
        // which create this background worker, which is just the UI thread)
        void OnCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            m_view.OnEnd();
        }

        void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            m_view.OnProgress(e.ProgressPercentage);
        }

        // note: run in thread pool
        // still not allowed to access the UI directly, the only method to do this is through
        // firing events
        void OnDoWork(object sender, DoWorkEventArgs e)
        {
            m_backgrdWorker.ReportProgress(0);
            for (int index = 0; index < m_total; ++index)
            {
                Thread.Sleep(m_intervalInMs);
                m_backgrdWorker.ReportProgress((int)(index * 100.0f / m_total));
            }
        }

        #endregion
    }
}
