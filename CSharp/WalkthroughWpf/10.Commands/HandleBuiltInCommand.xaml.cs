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

namespace _10.Commands
{
    /// <summary>
    /// Interaction logic for HandleBuiltInCommand.xaml
    /// </summary>
    public partial class HandleBuiltInCommand : Window
    {
        public HandleBuiltInCommand()
        {
            InitializeComponent();
        }

        private void HelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Demo how to deal with builtin command", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
