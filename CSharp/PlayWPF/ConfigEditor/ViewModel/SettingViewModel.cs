using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace ConfigEditor.ViewModel
{
    public sealed class SettingViewModel : ViewModelBase
    {
        // because the same viewmodel will also be used in "new setting dialog"
        // which should not trigger the message to enable "Save"
        private readonly bool _existing;

        // ---------------------------------------------------- //
        #region "bindable properties"

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set
            {
                if (_key == value) return;
                _key = value;
                RaisePropertyChanged("Key");
            }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                if (_value == value) return;
                _value = value;
                RaisePropertyChanged("Value");

                if (_existing)
                {
                    // just send out the message
                    // the message itself is important, not the value
                    Messenger.Default.Send(true);
                }
            }
        }

        #endregion

        // ---------------------------------------------------- //
        #region "constructor"

        public SettingViewModel(string key, string value, bool existing = true)
        {
            this.Key = key;
            this.Value = value;
            _existing = existing;
        }

        #endregion
    }
}
