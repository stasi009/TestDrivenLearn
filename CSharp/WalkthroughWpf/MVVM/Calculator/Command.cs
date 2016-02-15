using System;
using System.Windows.Input;

namespace MVVM.Calculator
{
    sealed class SquareRootCommand : ICommand
    {
        private readonly ViewModel m_viewmodel;

        public SquareRootCommand(ViewModel viewmodel)
        {
            m_viewmodel = viewmodel;
        }

        public void Execute(object parameter)
        {
            m_viewmodel.Result = Math.Sqrt(m_viewmodel.Number);
        }

        public bool CanExecute(object parameter)
        {
            return m_viewmodel.Number >= 0;
        }

        public event EventHandler CanExecuteChanged;
        public void NotifyCanExeChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
