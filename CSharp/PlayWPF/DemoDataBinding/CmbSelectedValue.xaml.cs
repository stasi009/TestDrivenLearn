using System.Collections.Generic;
using System.Windows;
using GalaSoft.MvvmLight;

namespace DemoDataBinding
{
    sealed class CmbSelectedValueViewModel : ViewModelBase
    {
        #region "notify-able properties"

        private IEnumerable<KeyValuePair<string, string>> _states;
        public IEnumerable<KeyValuePair<string, string>> States
        {
            get { return _states; }
            set
            {
                if (_states == value) return;
                _states = value;
                RaisePropertyChanged("States");
            }
        }

        private string _currentAbbreviation;
        public string CurrentAbbreviation
        {
            get { return _currentAbbreviation; }
            set
            {
                if (_currentAbbreviation == value) return;
                _currentAbbreviation = value;
                RaisePropertyChanged("CurrentAbbreviation");
            }
        }

        #endregion

        public CmbSelectedValueViewModel()
        {
            States = new Dictionary<string, string>
                         {
                             {"Washington","WA"},
                             {"South Carolina","SC"},
                             {"New York","NY"}
                         };
            CurrentAbbreviation = "WA";
        }
    }

    /// <summary>
    /// Interaction logic for CmbSelectedValue.xaml
    /// </summary>
    public partial class CmbSelectedValue : Window
    {
        public CmbSelectedValue()
        {
            InitializeComponent();
        }
    }
}
