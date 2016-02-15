using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Diagnostics;

namespace _07.Events
{
    /// <summary>
    /// Interaction logic for EventsFromCodes.xaml
    /// </summary>
    public partial class EventsFromCodes : Window
    {
        public EventsFromCodes()
        {
            InitializeComponent();

            grpbx1.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(GroupBox1_ButtonClick));
            grpbx2.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(GroupBox2_ButtonClick));
        }

        private void GroupBox1_ButtonClick(object sender, RoutedEventArgs e)
        {
            // note: the sender is that control which defines the event
            // but NOT the control which actually publishes out that event
            Debug.Assert(object.ReferenceEquals(sender, grpbx1));

            Button button = e.OriginalSource as Button;
            tbxDisplay.Text = string.Format("Button[{0}] Pressed.", button.Content);

            System.Media.SystemSounds.Exclamation.Play();
        }

        private void GroupBox2_ButtonClick(object sender, RoutedEventArgs e)
        {
            Debug.Assert(object.ReferenceEquals(sender, grpbx2));

            Button button = e.OriginalSource as Button;
            MessageBox.Show(button.Content.ToString(), "Button Pressed", MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }
    }
}
