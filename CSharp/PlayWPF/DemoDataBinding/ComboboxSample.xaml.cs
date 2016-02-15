using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using Utility;

namespace DemoDataBinding
{
    /// <summary>
    /// Interaction logic for ComboboxSample.xaml
    /// </summary>
    public partial class ComboboxSample : Window
    {
        internal sealed class ComboboxSampleViewModel : ViewModelBase
        {
            // ************************************************ //
            #region "notify-able properties"
            private PersonViewModel[] _people;
            public PersonViewModel[] People
            {
                get { return _people; }
                set
                {
                    if (_people == value) return;
                    _people = value;
                    RaisePropertyChanged("People");
                }
            }

            private PersonViewModel _currentPerson;
            public PersonViewModel CurrentPerson
            {
                get { return _currentPerson; }
                set
                {
                    if (_currentPerson == value) return;
                    _currentPerson = value;
                    RaisePropertyChanged("CurrentPerson");
                }
            }
            #endregion

            public ComboboxSampleViewModel()
            {
                this.People =
                    (from index in Enumerable.Range(1, 5)
                     select new PersonViewModel
                     {
                         Id = index,
                         Age = index * 10,
                         Name = string.Format("p{0}", index)
                     }).ToArray();
                this.CurrentPerson = People.Last();
            }
        }

        public ComboboxSample()
        {
            InitializeComponent();
            this.DataContext = new ComboboxSampleViewModel();
        }
    }
}
