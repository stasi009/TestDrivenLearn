using System;
using System.Windows.Input;
using EntityLib;

namespace MVVM.Students
{
    sealed class RemoveCommand : ICommand
    {
        private ViewModel m_viewModel;
        public RemoveCommand(ViewModel viewmodel)
        {
            m_viewModel = viewmodel;
        }

        public void Execute(object parameter)
        {
            m_viewModel.Remove(parameter as Student);
        }

        public bool CanExecute(object parameter)
        {
            Student selected = parameter as Student;
            return selected != null;
        }

        public event EventHandler CanExecuteChanged;
        public void FireCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
