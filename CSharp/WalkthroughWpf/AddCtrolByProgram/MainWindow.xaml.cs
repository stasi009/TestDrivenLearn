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

namespace AddCtrolByProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int m_index = 0;

        public MainWindow()
        {
            InitializeComponent();

            string content = string.Format("added in constructor,loaded index={0}", m_index);
            Button button = new Button { Content = content };
            AddToCanvas(button, 20, 20);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox txtbox = new TextBox
                                 {
                                     Text = string.Format("added in 'Loaded' event callback,loaded index={0}", m_index),
                                     Padding = new Thickness(10, 20, 10, 20)
                                 };
            AddToCanvas(txtbox, 20, 60);
        }

        private void AddToCanvas(Control ctrl, double left, double top)
        {
            ++m_index;
            Canvas.SetLeft(ctrl, left);
            Canvas.SetTop(ctrl, top);
            canvas.Children.Add(ctrl);
        }
    }
}
