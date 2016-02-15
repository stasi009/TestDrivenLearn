using System.Collections.ObjectModel;
using System.Linq;
using ConfigEditor.DataAccess;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace ConfigEditor.ViewModel
{
    public sealed class ChosenMeasurementViewModel : ViewModelBase
    {
        #region "properties"

        public ObservableCollection<MeasurementViewModel> ChosenMeasurements { get; private set; }
        private bool _selectAll;

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex == value) return;
                _selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }

        #endregion

        #region "constructor"

        public ChosenMeasurementViewModel(ConfigRepository configRepository, MeasRepository measRepository)
        {
            // there may be duplicate keys in "InputMeasurementKeys"
            // but for each key, there is only one Measurement related
            ChosenMeasurements = new ObservableCollection<MeasurementViewModel>(
                from key in configRepository.InputMeasurementKeys
                join meas in measRepository.Measurements
                on key equals meas.Key
                select new MeasurementViewModel(meas));

            DeselectCmd = new RelayCommand(OnDeselect);
            SelectAllCmd = new RelayCommand(OnSelectAll);

            UpCmd = new RelayCommand(OnMoveUp, () => this.SelectedIndex > 0);
            DownCmd = new RelayCommand(OnMoveDown, () => this.SelectedIndex < ChosenMeasurements.Count - 1);
        }

        #endregion

        #region "commands"

        public RelayCommand DeselectCmd { get; private set; }
        private void OnDeselect()
        {
            var itemsToRemove = (from mvm in ChosenMeasurements
                                 where mvm.IsSelectToRemove
                                 select mvm).ToArray();
            foreach (var item in itemsToRemove)
            {
                ChosenMeasurements.Remove(item);
            }
            Messenger.Default.Send(true);
        }

        public RelayCommand SelectAllCmd { get; private set; }
        private void OnSelectAll()
        {
            _selectAll = !_selectAll;
            foreach (var mvm in ChosenMeasurements)
            {
                mvm.IsSelectToRemove = _selectAll;
            }
        }

        public RelayCommand UpCmd { get; private set; }
        private void OnMoveUp()
        {
            ChosenMeasurements.Move(this.SelectedIndex, this.SelectedIndex - 1);
            Messenger.Default.Send(true);
        }

        public RelayCommand DownCmd { get; private set; }
        private void OnMoveDown()
        {
            ChosenMeasurements.Move(this.SelectedIndex, this.SelectedIndex + 1);
            Messenger.Default.Send(true);
        }

        #endregion

        #region "public APIs"

        public void Add(MeasurementViewModel measurement)
        {
            ChosenMeasurements.Add(measurement);
            Messenger.Default.Send(true);
        }

        public void Save(ConfigRepository configRepository)
        {
            configRepository.InputMeasurementKeys = from mvm in ChosenMeasurements
                                                    select mvm.Key;
        }

        #endregion
    }
}
