using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AsyncWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BeginInvokeDemo : Window, IView
    {
        #region "################################## member fields"

        private BeginInvokePresenter m_presenter;

        #endregion

        public BeginInvokeDemo()
        {
            InitializeComponent();

            m_presenter = new BeginInvokePresenter(10,200,this);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            m_presenter.Start();
        }

        private void SafeInvokeControls(Action action)
        {
            if (CheckAccess())
            {
                action();
            }
            else
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, action);
            }
        }

        #region "################################## IView implementation"

        public void OnStart()
        {
            SafeInvokeControls(() =>
                                   {
                                       btnStart.IsEnabled = false;
                                       lblProgress.Content = "0%";
                                       progBar.Value = 0;
                                   });
        }

        public void OnProgress(int percentage)
        {
            SafeInvokeControls(() =>
                                   {
                                       progBar.Value = percentage;
                                       lblProgress.Content = string.Format("{0}%", percentage);
                                   });
        }

        public void OnEnd()
        {
            SafeInvokeControls(() =>
                                   {
                                       btnStart.IsEnabled = true;
                                       lblProgress.Content = "100%";
                                       progBar.Value = 100;
                                   });
        }

        #endregion
    }
}
