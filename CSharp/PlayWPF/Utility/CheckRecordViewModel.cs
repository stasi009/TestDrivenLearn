using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace Utility
{
    public sealed class CheckRecordViewModel : ViewModelBase
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
                Messenger.Default.Send(this);
            }
        }

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

        public CheckRecordViewModel(string name)
        {
            IsSelected = false;
            Name = name;
        }
    }
}
