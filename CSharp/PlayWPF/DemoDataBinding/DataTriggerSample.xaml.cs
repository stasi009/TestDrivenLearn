using System.Windows;
using GalaSoft.MvvmLight;

namespace DemoDataBinding
{
    /// <summary>
    /// Interaction logic for DataTriggerSample.xaml
    /// </summary>
    public partial class DataTriggerSample : Window
    {
        // ******************************************** //
        #region "inner classes"

        enum Closeness
        {
            Family,
            GirlFriend,
            Friend,
            NotCare
        }

        private sealed class PersonViewModel : ViewModelBase
        {
            private string _name;
            public string Name
            {
                get { return _name; }
                set
                {
                    if (_name == value) return;
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }

            private Closeness _closeness;
            public Closeness Closeness
            {
                get { return _closeness; }
                set
                {
                    if (_closeness == value) return;
                    _closeness = value;
                    RaisePropertyChanged("Closeness");
                }
            }

            public PersonViewModel(string name, Closeness closeness)
            {
                this.Name = name;
                this.Closeness = closeness;
            }
        }

        private sealed class MainViewModel : ViewModelBase
        {
            private readonly PersonViewModel[] _relationships;
            private readonly Closeness[] _allCloseness;

            private PersonViewModel _current;
            public PersonViewModel Current
            {
                get { return _current; }
                set
                {
                    if (_current == value) return;
                    _current = value;
                    RaisePropertyChanged("Current");
                }
            }

            public MainViewModel()
            {
                _relationships = new PersonViewModel[]
                                     {
                                         new PersonViewModel("Dad",Closeness.Family),
                                         new PersonViewModel("Mom",Closeness.Family),
                                         new PersonViewModel("Shuting",Closeness.GirlFriend),
                                         new PersonViewModel("Wenmao",Closeness.Friend), 
                                         new PersonViewModel("Tom",Closeness.NotCare), 
                                         new PersonViewModel("Mary",Closeness.NotCare), 
                                     };
                this.Current = _relationships[2];

                _allCloseness = new Closeness[]
                                    {
                                        Closeness.Family, 
                                        Closeness.GirlFriend, 
                                        Closeness.Friend, 
                                        Closeness.NotCare, 
                                    };
            }

            public PersonViewModel[] Relationships
            {
                get { return _relationships; }
            }

            public Closeness[] AllCloseness
            {
                get { return _allCloseness; }
            }
        }

        #endregion

        public DataTriggerSample()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
