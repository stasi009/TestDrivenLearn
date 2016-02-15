using System.Windows;

namespace ConfigEditor.View
{
    /// <summary>
    /// Interaction logic for NewSettingDlg.xaml
    /// </summary>
    public partial class NewSettingDlg : Window
    {
        public NewSettingDlg()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
