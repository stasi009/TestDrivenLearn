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
using System.Windows.Shapes;

namespace _07.Events
{
    /// <summary>
    /// Interaction logic for StopRouting.xaml
    /// </summary>
    public partial class StopRouting : Window
    {
        public StopRouting()
        {
            InitializeComponent();
        }

        private void Grid_ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;
            MessageBox.Show(string.Format("Grid Level [{0}] Pressed", button.Content),
                            "Grid Level Handler", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnRouting_Click(object sender, RoutedEventArgs e)
        {
            // note: pay attention to the order, bubbling events
            // event handler attached to the local control fired first
            // then is the event handler attached to its parent container
            MessageBox.Show("Button Level, [Routing] Clicked, waiting global one fired.");
        }

        private void btnStopRouting_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Button Level, [NoRouting] Clicked, NO global one fired.");
            // note: set "Handled" to true to prevent further routing
            // same for tunneling down events
            e.Handled = true;
        }
    }
}
