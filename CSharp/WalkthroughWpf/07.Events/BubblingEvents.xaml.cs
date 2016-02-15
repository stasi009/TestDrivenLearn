using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace _07.Events
{
    /// <summary>
    /// Interaction logic for BubblingEvents.xaml
    /// </summary>
    public partial class BubblingEvents : Window
    {
        public BubblingEvents()
        {
            InitializeComponent();
        }

        private void GroupBox1_ButtonClick(object sender, RoutedEventArgs e)
        {
            // note: the sender is that control which defines the event
            // but NOT the control which actually publishes out that event
            // you should access 'OriginalSource' to get the control which actually invokes the event
            Debug.Assert(object.ReferenceEquals(sender,grpbx1));

            Button button = e.OriginalSource as Button;
            tbxDisplay.Text = string.Format("Button[{0}] Pressed.",button.Content);
        }

        private void GroupBox2_ButtonClick(object sender, RoutedEventArgs e)
        {
            Debug.Assert(object.ReferenceEquals(sender,grpbx2));

            Button button = e.OriginalSource as Button;
            MessageBox.Show(button.Content.ToString(), "Button Pressed", MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }
    }
}
