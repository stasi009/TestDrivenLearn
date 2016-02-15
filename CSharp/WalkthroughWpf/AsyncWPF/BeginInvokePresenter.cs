using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncWPF
{
    sealed class BeginInvokePresenter : CounterPresenterBase
    {
        public BeginInvokePresenter(int total, int intervalInMs, IView view)
            : base(total, intervalInMs, view)
        {
        }

        public override void Start()
        {
            Task.Factory.StartNew(() =>
                                      {
                                          m_view.OnStart();
                                          for (int index = 0; index < m_total; ++index)
                                          {
                                              Thread.Sleep(m_intervalInMs);
                                              m_view.OnProgress((int)(index * 100.0f / m_total));
                                          }
                                          m_view.OnEnd();
                                      });
        }
    }
}
