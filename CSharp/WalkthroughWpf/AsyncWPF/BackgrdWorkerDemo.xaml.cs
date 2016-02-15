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
    public partial class BackgrdWorkerDemo : Window, IView
    {
        #region "################################## member fields"

        private readonly IPresenter m_presenter;

        #endregion

        public BackgrdWorkerDemo()
        {
            InitializeComponent();

            m_presenter = new BackgrdWorkerPresenter(10, 200, this);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            m_presenter.Start();
        }

        #region "################################## IView implementation"

        // note: because these methods are invoked from BackgroundWorker
        // they are allowed to access the UI directly
        public void OnStart()
        {
            btnStart.IsEnabled = false;
            lblProgress.Content = "0%";
            progBar.Value = 0;
        }

        public void OnProgress(int percentage)
        {
            progBar.Value = percentage;
            lblProgress.Content = string.Format("{0}%", percentage);
        }

        public void OnEnd()
        {
            btnStart.IsEnabled = true;
            lblProgress.Content = "100%";
            progBar.Value = 100;
        }

        #endregion
    }
}
