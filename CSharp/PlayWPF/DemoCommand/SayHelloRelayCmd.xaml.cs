using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DemoCommand
{
    internal sealed class SayHelloRelayCmdViewModel : ViewModelBase
    {
        #region "notify-able properties"

        private int _counter;
        public int Counter
        {
            get { return _counter; }
            set
            {
                if (_counter == value) return;
                _counter = value;
                RaisePropertyChanged("Counter");

                // note: In WPF, below codes is not required, 
                // because the CommandManager (provided by WPF) 
                // will automatically requery the state of the commands 
                // when the user clicks on the UI
                // in SilverLight, we need below codes, because Silverlight doeesn't have
                // CommandManager
                // SayhelloCmd.RaiseCanExecuteChanged();
            }
        }

        #endregion

        public RelayCommand<string> SayhelloCmd { get; private set; }
        public ICommand IncrementCmd { get; private set; }

        public SayHelloRelayCmdViewModel()
        {
            Counter = 0;
            IncrementCmd = new RelayCommand(() => { Counter++; });
            SayhelloCmd = new RelayCommand<string>(
                txt=> MessageBox.Show("Hello, "+txt),
                _=>Counter%2==0);
        }
    }

    /// <summary>
    /// Interaction logic for SayHelloRelayCmd.xaml
    /// </summary>
    public partial class SayHelloRelayCmd : Window
    {
        public SayHelloRelayCmd()
        {
            InitializeComponent();
            this.DataContext = new SayHelloRelayCmdViewModel();
        }
    }
}
