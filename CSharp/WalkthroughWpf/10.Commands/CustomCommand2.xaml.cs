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
    /// Interaction logic for CustomCommand2.xaml
    /// </summary>
    public partial class CustomCommand2 : Window
    {
        private static readonly RoutedUICommand m_testCommand = new RoutedUICommand();

        public static RoutedUICommand TestCommand
        {
            get { return m_testCommand; }
        }

        public CustomCommand2()
        {
            InitializeComponent();
        }

        private void TestCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            RichTextBox editor = sender as RichTextBox;
            editor.AppendText("hello wpf from cheka\n");
        }
    }
}
