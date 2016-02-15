using System.Windows.Controls;
using ConfigEditor.ViewModel;

namespace ConfigEditor.View
{
    /// <summary>
    /// Interaction logic for UserCtrlOtherSettings.xaml
    /// </summary>
    public partial class UserCtrlOtherSettings : UserControl, IOtherSettingsView
    {
        public UserCtrlOtherSettings()
        {
            InitializeComponent();
        }

        public bool WaitUserInput(SettingViewModel newSetting)
        {
            NewSettingDlg dlg = new NewSettingDlg { DataContext = newSetting };
            return (bool)dlg.ShowDialog();
        }
    }
}
