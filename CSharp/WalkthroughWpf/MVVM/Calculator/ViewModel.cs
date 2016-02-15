using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace MVVM.Calculator
{
    sealed class ViewModel : INotifyPropertyChanged
    {
        #region "event definition"

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public ViewModel()
        {
            m_sqrtCmd = new SquareRootCommand(this);
        }

        #region "properties"

        private double m_number;
        public double Number
        {
            get { return m_number; }
            set
            {
                if (m_number != value)
                {
                    m_number = value;
                    NotifyPropertyChanged("Number");
                    m_sqrtCmd.NotifyCanExeChanged();

                    if (m_number < 0)
                        this.Result = double.NaN;
                }
            }
        }

        private double m_result;
        public double Result
        {
            get { return m_result; }
            set
            {
                if (m_result != value)
                {
                    m_result = value;
                    NotifyPropertyChanged("Result");
                }
            }
        }

        #endregion

        #region "commands"

        private readonly SquareRootCommand m_sqrtCmd;
        public ICommand SqrtCommand
        {
            get { return m_sqrtCmd; }
        }

        #endregion

        #region "helper methods"

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
