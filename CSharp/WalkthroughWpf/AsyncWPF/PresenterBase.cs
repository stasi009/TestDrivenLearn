using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncWPF
{
    interface IPresenter
    {
        void Start();
    }

    abstract  class CounterPresenterBase : IPresenter
    {
        #region "*************** member fields ***************"

        protected readonly int m_total;
        protected readonly int m_intervalInMs;// unit: ms
        protected readonly IView m_view;

        #endregion

        #region "*************** constructors ***************"

        public CounterPresenterBase(int total, int intervalInMs, IView view)
        {
            m_total = total;
            m_intervalInMs = intervalInMs;
            m_view = view;
        }

        #endregion

        #region "*************** abstract methods ***************"

        public abstract void Start();

        #endregion
    }
}
