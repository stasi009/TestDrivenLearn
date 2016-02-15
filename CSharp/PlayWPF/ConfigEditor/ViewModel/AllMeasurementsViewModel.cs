using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using ConfigEditor.DataAccess;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ConfigEditor.ViewModel
{
    public sealed class AllMeasurementsViewModel : ViewModelBase
    {
        #region "member fields"

        private readonly MeasurementViewModel[] _allMeasurements;
        private readonly ObservableCollection<MeasurementViewModel> _filtered;
        private readonly ChosenMeasurementViewModel _vmChosenMeasurements;
        private bool _selectAll;

        #endregion

        #region "bindable properties"

        public ICollectionView MeasurementsView { get; private set; }

        public string[] SignalTypeOptions { get; private set; }

        private string _currentSignalType;
        public string CurrentSignalType
        {
            get { return _currentSignalType; }
            set
            {
                if (_currentSignalType == value) return;
                _currentSignalType = value;
                RaisePropertyChanged("CurrentSignalType");
            }
        }

        private string _devSearchTxt;
        public string DevSearchTxt
        {
            get { return _devSearchTxt; }
            set { _devSearchTxt = value.ToLower(); }
        }

        #endregion

        #region "commands"

        public RelayCommand FilterCmd { get; private set; }
        private void OnFilter()
        {
            _filtered.Clear();

            foreach (var m in _allMeasurements)
            {
                bool devmatch = !(!string.IsNullOrEmpty(_devSearchTxt) && !m.Device.ToLower().Contains(_devSearchTxt));

                bool signalmatch = !(_currentSignalType != "All" && m.SignalType != _currentSignalType);

                if (devmatch && signalmatch)
                {
                    m.IsSelectToAdd = false;
                    _filtered.Add(m);
                }
            }
        }

        public RelayCommand SelectAllCmd { get; private set; }
        private void OnSelectAll()
        {
            _selectAll = !_selectAll;
            foreach (var m in _filtered)
            {
                m.IsSelectToAdd = _selectAll;
            }
        }

        public RelayCommand AddCmd { get; private set; }
        private void OnAdd()
        {
            var itemsToAdd = from mvm in _filtered
                             where mvm.IsSelectToAdd
                             select mvm;
            foreach (var mvm in itemsToAdd)
            {
                _vmChosenMeasurements.Add(mvm);
            }
        }

        #endregion

        #region "constructor"

        public AllMeasurementsViewModel(MeasRepository measRepository, ChosenMeasurementViewModel vmChosenMeasurements)
        {
            _allMeasurements = (from m in measRepository.Measurements
                                select new MeasurementViewModel(m)).ToArray();

            _filtered = new ObservableCollection<MeasurementViewModel>(_allMeasurements);

            MeasurementsView = CollectionViewSource.GetDefaultView(_filtered);
            MeasurementsView.SortDescriptions.Add(new SortDescription("Device", ListSortDirection.Ascending));
            MeasurementsView.SortDescriptions.Add(new SortDescription("SignalType", ListSortDirection.Descending));
            MeasurementsView.GroupDescriptions.Add(new PropertyGroupDescription("Device"));

            SignalTypeOptions = Enumerable.Repeat("All", 1).Concat(_allMeasurements.Select(m => m.SignalType).Distinct().OrderBy(s => s)).ToArray();
            CurrentSignalType = SignalTypeOptions[0];

            FilterCmd = new RelayCommand(OnFilter);
            SelectAllCmd = new RelayCommand(OnSelectAll);
            AddCmd = new RelayCommand(OnAdd);

            _vmChosenMeasurements = vmChosenMeasurements;
        }

        #endregion
    }
}
