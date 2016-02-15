using System;
using System.Windows.Input;

namespace MVVM.Students
{
    sealed class SaveCommand : ICommand
    {
        private ViewModel m_viewModel;

        public SaveCommand(ViewModel viewmodel)
        {
            m_viewModel = viewmodel;
        }

        public void Execute(object parameter)
        {
            m_viewModel.Save();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
