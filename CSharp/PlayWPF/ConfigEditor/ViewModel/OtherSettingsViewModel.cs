using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using ConfigEditor.DataAccess;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace ConfigEditor.ViewModel
{
    public interface IOtherSettingsView
    {
        bool WaitUserInput(SettingViewModel newSetting);
    }

    public sealed class OtherSettingsViewModel : ViewModelBase
    {
        // ********************************************* //
        #region "member fields"
        private readonly ObservableCollection<SettingViewModel> _otherSettings;
        private readonly IOtherSettingsView _view;
        #endregion

        // ********************************************* //
        #region "bindable properties"

        public ICollectionView SettingsView { get; private set; }

        #endregion

        // ********************************************* //
        #region "constructor"

        public OtherSettingsViewModel(ConfigRepository configRepository, IOtherSettingsView view)
        {
            _view = view;

            _otherSettings = new ObservableCollection<SettingViewModel>();
            foreach (var kv in configRepository.OtherSettings)
            {
                _otherSettings.Add(new SettingViewModel(kv.Key, kv.Value));
            }
            SettingsView = CollectionViewSource.GetDefaultView(_otherSettings);
            SettingsView.SortDescriptions.Add(new SortDescription("Key", ListSortDirection.Ascending));

            AddCmd = new RelayCommand(OnAdd);
            RemoveCmd = new RelayCommand<object>(OnRemove);
        }

        #endregion

        // ********************************************* //
        #region "commands"

        public RelayCommand AddCmd { get; private set; }
        private void OnAdd()
        {
            SettingViewModel newSetting = new SettingViewModel("", "", false);
            if (_view.WaitUserInput(newSetting))
            {
                _otherSettings.Add(newSetting);
                Messenger.Default.Send(true);
            }
        }

        public RelayCommand<object> RemoveCmd { get; private set; }
        private void OnRemove(object parm)
        {
            var itemsToRemove = ((IList)parm).Cast<SettingViewModel>().ToArray();

            foreach (var item in itemsToRemove)
            {
                _otherSettings.Remove(item);
            }

            Messenger.Default.Send(true);
        }

        #endregion

        // ********************************************* //
        #region "public API"

        public void Save(ConfigRepository configRepository)
        {
            configRepository.OtherSettings =
                from setting in _otherSettings
                select new KeyValuePair<string, string>(setting.Key, setting.Value);
        }

        #endregion
    }
}
