using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace MVVM.SKI
{
    sealed class SaveCommand : ICommand
    {
        private readonly ViewModel m_viewModel;

        public SaveCommand(ViewModel viewModel)
        {
            m_viewModel = viewModel;
            m_viewModel.PropertyChanged += (sender, evtargs) =>
                                               {
                                                   if (evtargs.PropertyName.Equals("SelectedCompetitor")
                                                       && CanExecuteChanged != null)
                                                       CanExecuteChanged(this, EventArgs.Empty);
                                               };
        }

        public void Execute(object parameter)
        {
            m_viewModel.SaveSelectedCompetitor();
        }

        public bool CanExecute(object parameter)
        {
            return m_viewModel.SelectedCompetitor != null;
        }

        public event EventHandler CanExecuteChanged;
    }
}
