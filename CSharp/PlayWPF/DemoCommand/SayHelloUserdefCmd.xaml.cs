using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;

namespace DemoCommand
{
    internal sealed class UserdefSayHelloCmd : ICommand
    {
        private readonly SayHelloUserdefCmdViewModel _owner;

        public UserdefSayHelloCmd(SayHelloUserdefCmdViewModel owner)
        {
            _owner = owner;
            _owner.PropertyChanged += (sender, evtargs) =>
            {
                if (CanExecuteChanged != null && evtargs.PropertyName.Equals("Counter"))
                {
                    // tell WPF to re-evaluate the "CanExecute" property
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            };
        }

        public bool CanExecute(object parameter)
        {
            return _owner != null && _owner.Counter % 2 == 0;
        }

        public void Execute(object parameter)
        {
            MessageBox.Show("Hello, " + parameter);
        }

        public event EventHandler CanExecuteChanged;
    }

    internal sealed class UserdefIncrementCmd : ICommand
    {
        private readonly SayHelloUserdefCmdViewModel _owner;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _owner.Counter++;
        }

        public event EventHandler CanExecuteChanged;

        public UserdefIncrementCmd(SayHelloUserdefCmdViewModel owner)
        {
            _owner = owner;
        }
    }

    internal sealed class SayHelloUserdefCmdViewModel : ViewModelBase
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
            }
        }

        #endregion

        public ICommand SayhelloCmd { get; private set; }
        public ICommand IncrementCmd { get; private set; }

        public SayHelloUserdefCmdViewModel()
        {
            this.Counter = 0;
            this.SayhelloCmd = new UserdefSayHelloCmd(this);
            this.IncrementCmd = new UserdefIncrementCmd(this);
        }
    }

    /// <summary>
    /// Interaction logic for SayHelloUserdefCmd.xaml
    /// </summary>
    public partial class SayHelloUserdefCmd : Window
    {
        public SayHelloUserdefCmd()
        {
            InitializeComponent();
            this.DataContext = new SayHelloUserdefCmdViewModel();
        }
    }
}
