using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AsyncWPF
{
    /// <summary>
    /// Interaction logic for DispatcherTimerDemo.xaml
    /// </summary>
    public partial class DispatcherTimerDemo : Window
    {
        // chekatodo: Do I need to dispose this timer, but this timer doesn't implement IDisposable??
        private readonly DispatcherTimer m_timer;
        private readonly Random m_random;

        public DispatcherTimerDemo()
        {
            InitializeComponent();

            m_random = new Random();

            m_timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            m_timer.Tick += this.OnTimerTicked;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            m_timer.Start();
        }

        private void OnTimerTicked(object sender, EventArgs evtargs)
        {
            byte[] bytes = new byte[3];
            m_random.NextBytes(bytes);
            Color color = Color.FromRgb(bytes[0], bytes[1], bytes[2]);

            lblTime.Content = DateTime.Now.ToLongTimeString();
            lblTime.Foreground = new SolidColorBrush(color);
        }
    }
}
