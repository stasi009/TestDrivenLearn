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
    /// Interaction logic for HandledEventsToo.xaml
    /// </summary>
    public partial class HandledEventsToo : Window
    {
        public HandledEventsToo()
        {
            InitializeComponent();

            grpbxPrevent.AddHandler(Button.ClickEvent, new RoutedEventHandler(Groupbox_ButtonClicked));

            // note: the last parameter indicates that the upper event handler will still be fired
            // even if that event has been marked as 'Handled' in lower event handler
            grpbxHandledToo.AddHandler(Button.ClickEvent, new RoutedEventHandler(Groupbox_ButtonClicked), true);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            MessageBox.Show(button.Content.ToString(), "Button Level Handler", MessageBoxButton.OK,
                            MessageBoxImage.Information);
            e.Handled = true;
        }

        private void Groupbox_ButtonClicked(object sender, RoutedEventArgs e)
        {
            System.Media.SystemSounds.Beep.Play();
        }
    }
}
