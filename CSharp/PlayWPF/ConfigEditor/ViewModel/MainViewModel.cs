using ConfigEditor.DataAccess;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace ConfigEditor.ViewModel
{
    public interface IMainWindow
    {
        bool? SaveOrNot();
    }

    public sealed class MainViewModel : ViewModelBase
    {
        // ********************************************* //
        #region "member fields"

        private readonly MeasRepository _measRepository;
        private readonly ConfigRepository _configRepository;
        private readonly OtherSettingsViewModel _vmOtherSettings;
        private readonly AllMeasurementsViewModel _vmAllMeasurements;
        private readonly ChosenMeasurementViewModel _vmChosenMeasurement;
        private bool _dirty;
        private readonly IMainWindow _view;

        #endregion

        // ********************************************* //
        #region "commands"

        public RelayCommand SaveCmd { get; private set; }

        private void Save()
        {
            _vmOtherSettings.Save(_configRepository);
            _vmChosenMeasurement.Save(_configRepository);
            _configRepository.Save();
            _dirty = false;
        }

        #endregion

        // ********************************************* //
        #region "bindable properties"

        public OtherSettingsViewModel OtherSettingsViewModel
        {
            get { return _vmOtherSettings; }
        }

        public AllMeasurementsViewModel AllMeasurementsViewModel
        {
            get { return _vmAllMeasurements; }
        }

        public ChosenMeasurementViewModel ChosenMeasurementViewModel
        {
            get { return _vmChosenMeasurement; }
        }

        #endregion

        // ********************************************* //
        #region "constructor"

        public MainViewModel(IMainWindow view, IOtherSettingsView viewOtherSettings)
        {
            _view = view;

            _measRepository = new MeasRepository("data/measurements.csv");
            _configRepository = new ConfigRepository("data/OmsLite.exe.config");

            _vmOtherSettings = new OtherSettingsViewModel(_configRepository, viewOtherSettings);
            _vmChosenMeasurement = new ChosenMeasurementViewModel(_configRepository, _measRepository);
            _vmAllMeasurements = new AllMeasurementsViewModel(_measRepository, _vmChosenMeasurement);

            // ----------------- commands
            SaveCmd = new RelayCommand(Save, () => _dirty);

            // ----------------- messages
            Messenger.Default.Register<bool>(this, _ => { _dirty = true; });
        }

        #endregion

        // ********************************************* //
        #region "public API"

        public bool NeedCancel()
        {
            if (_dirty)
            {
                bool? save = _view.SaveOrNot();
                if (save.HasValue)
                {
                    if ((bool)save)
                    {
                        Save();
                    }
                    return false;
                }
                return true;// canceled
            }
            return false;
        }

        #endregion

    }
}
