using ConfigEditor.DataAccess;
using GalaSoft.MvvmLight;

namespace ConfigEditor.ViewModel
{
    public sealed class MeasurementViewModel : ViewModelBase
    {
        private readonly Measurement _measurement;

        public MeasurementViewModel(Measurement m)
        {
            _measurement = m;
        }

        public string Key { get { return _measurement.Key; } }
        public string SignalReference { get { return _measurement.SignalReference; } }
        public string Device { get { return _measurement.Device; } }
        public string SignalType { get { return _measurement.SignalType; } }
        public string PhasorType { get { return _measurement.PhasorType; } }

        private bool _isSelectToAdd;
        public bool IsSelectToAdd
        {
            get { return _isSelectToAdd; }
            set
            {
                if (_isSelectToAdd == value) return;
                _isSelectToAdd = value;
                RaisePropertyChanged("IsSelectToAdd");
            }
        }

        private bool _isSelectToRemove;
        public bool IsSelectToRemove
        {
            get { return _isSelectToRemove; }
            set
            {
                if (_isSelectToRemove == value) return;
                _isSelectToRemove = value;
                RaisePropertyChanged("IsSelectToRemove");
            }
        }
    }//MeasurementViewModel
}
