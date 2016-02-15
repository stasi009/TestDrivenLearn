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
    /// Interaction logic for CustomCommands.xaml
    /// </summary>
    public partial class CustomCommands : Window
    {
        public static RoutedCommand TestCommand = new RoutedCommand();

        static CustomCommands()
        {
            // note: add composite shortkey to command
            TestCommand.InputGestures.Add(new KeyGesture(Key.T, ModifierKeys.Alt));
        }

        public CustomCommands()
        {
            InitializeComponent();
        }

        private void TestCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Test Custom Commands", "Test", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
