using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Utility
{
    public sealed class IncrementRecordViewModel : ViewModelBase
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

        private int _number;
        public int Number
        {
            get { return _number; }
            set
            {
                if (_number == value) return;
                _number = value;
                RaisePropertyChanged("Number");
            }
        }

        public RelayCommand IncrementCmd { get; private set; }

        public IncrementRecordViewModel(string name)
        {
            Name = name;
            Number = 0;

            IncrementCmd = new RelayCommand(() =>
                                                {
                                                    Number++;
                                                });
        }
    }
}
