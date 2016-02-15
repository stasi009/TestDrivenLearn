using System;
using System.ComponentModel;
using System.Windows;
using ConfigEditor.ViewModel;

namespace ConfigEditor.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        private MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel = new MainViewModel(this,viewOtherSettings);
            this.DataContext = _viewModel;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = _viewModel.NeedCancel();
        }

        public bool? SaveOrNot()
        {
            var userchoice = MessageBox.Show("Configuration has been changed, save or not?", "Confirm", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (userchoice)
            {
                case MessageBoxResult.Yes:
                    return true;

                case MessageBoxResult.No:
                    return false;

                case MessageBoxResult.Cancel:
                    return null;

                default:
                    throw new InvalidOperationException("impossible choice");
            }
        }
    }
}
