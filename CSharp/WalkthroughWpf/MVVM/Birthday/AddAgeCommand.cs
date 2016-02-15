using System;
using System.Windows.Input;

namespace MVVM.Birthday
{
    class AddAgeCommand : ICommand
    {
        private readonly PersonViewModel m_viewModel;

        public AddAgeCommand(PersonViewModel viewModel)
        {
            m_viewModel = viewModel;
        }

        #region [ implement ICommand ]
        public void Execute(object parameter)
        {
            ++m_viewModel.Age;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        #endregion
    }
}
