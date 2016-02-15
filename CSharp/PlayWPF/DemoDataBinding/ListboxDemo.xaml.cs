using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Utility;

namespace DemoDataBinding
{
    /// <summary>
    /// Interaction logic for ListboxDemo.xaml
    /// </summary>
    public partial class ListboxDemo : Window
    {
        // ########################################## //
        #region "ViewModels"

        private sealed class MainViewModel : ViewModelBase
        {
            #region "notify-able properties"
            private IEnumerable<CheckRecordViewModel> _people;
            public IEnumerable<CheckRecordViewModel> People
            {
                get { return _people; }
                set
                {
                    if (_people == value) return;
                    _people = value;
                    RaisePropertyChanged("People");
                }
            }
            #endregion

            #region "commands"

            public RelayCommand CheckCommand { get; private set; }

            #endregion

            public MainViewModel()
            {
                var names = new[] { "Tom", "Dick", "Mary" };
                People = (from name in names
                          select new CheckRecordViewModel(name)).ToArray();

                CheckCommand = new RelayCommand(
                    CheckCmdExecute,
                    () => People.Any(p => p.IsSelected));
            }

            private void CheckCmdExecute()
            {
                string msg = People.Where(p => p.IsSelected).Select(p => p.Name).Aggregate(
                    new StringBuilder(),
                    (sb, name) => sb.Append(name).Append(","),
                    sb => sb.ToString());
                MessageBox.Show(msg);
            }
        }

        #endregion

        // ########################################## //
        public ListboxDemo()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
