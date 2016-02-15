using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using Utility;

namespace DemoDataBinding
{
    /// <summary>
    /// Interaction logic for ListViewSimple.xaml
    /// </summary>
    public partial class ListViewSimple : Window
    {
        private sealed class MainViewModel : ViewModelBase
        {
            private IncrementRecordViewModel[] _records;
            public IncrementRecordViewModel[] Records
            {
                get { return _records; }
                set
                {
                    if (_records == value) return;
                    _records = value;
                    RaisePropertyChanged("Records");
                }
            }

            public MainViewModel()
            {
                string[] names = new string[] { "tom", "mike", "alice", "mary" };
                Records = (from name in names
                           select new IncrementRecordViewModel(name)).ToArray();
            }
        }

        public ListViewSimple()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
