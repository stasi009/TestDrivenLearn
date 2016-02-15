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

namespace _06.ResourceStyle.Style
{
    /// <summary>
    /// Interaction logic for EventSetter.xaml
    /// </summary>
    public partial class EventSetter : Window
    {
        public EventSetter()
        {
            InitializeComponent();
        }

        private void OnButtonClicked(object  sender,RoutedEventArgs evtargs)
        {
            Button pressedButton = sender as Button;
            tbxDisplay.Text = pressedButton.Content.ToString();
        }
    }
}
