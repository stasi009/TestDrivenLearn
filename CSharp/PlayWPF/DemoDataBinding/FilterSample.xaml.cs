using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace DemoDataBinding
{
    /// <summary>
    /// Interaction logic for FilterSample.xaml
    /// </summary>
    public partial class FilterSample : Window
    {
        // ------------------------------------------------ //
        #region "inner classes"

        private sealed class OptionViewModel : ViewModelBase
        {
            #region "bindable properties"

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

            private bool _isSelected;
            public bool IsSelected
            {
                get { return _isSelected; }
                set
                {
                    if (_isSelected == value) return;
                    _isSelected = value;
                    RaisePropertyChanged("IsSelected");

                    // the data contained in the message is not important
                    // just occupy the position
                    Messenger.Default.Send(0);
                }
            }

            #endregion
        }

        private sealed class MainViewModel : ViewModelBase
        {
            public IEnumerable<OptionViewModel> AllOptions { get; private set; }
            public CollectionViewSource SelectedOptions { get; private set; }

            public MainViewModel()
            {
                var alloptions = new List<OptionViewModel>();
                for (int index = 0; index < 5; index++)
                {
                    alloptions.Add(new OptionViewModel
                                       {
                                           Name = string.Format("Option{0}", index + 1),
                                           IsSelected = index % 2 == 0
                                       });
                }
                this.AllOptions = alloptions;

                this.SelectedOptions = new CollectionViewSource
                                           {
                                               Source = alloptions
                                           };
                this.SelectedOptions.Filter += (sender, evtargs) =>
                {
                    OptionViewModel option = (OptionViewModel)evtargs.Item;
                    evtargs.Accepted = option.IsSelected;
                };

                Messenger.Default.Register<int>(this, _ => SelectedOptions.View.Refresh());
            }


        }// MainViewModel

        #endregion

        // ------------------------------------------------ //
        public FilterSample()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
