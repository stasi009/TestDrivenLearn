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

namespace _16.Navigation
{
    /// <summary>
    /// Interaction logic for PassStatePage1.xaml
    /// </summary>
    public partial class PassStatePage1 : Page
    {
        private static int m_counter = 0;

        public PassStatePage1()
        {
            InitializeComponent();

            // note: every time navigated back, a totally new page object is created
            // so state stored in instance field will not be kept
            // note: but some controls on this page can keep state 
            // (especially when use builtin that "browse back" button), however, that doesn't mean
            // the background page object remains the same
            ++m_counter;
            lblCounter.Content = string.Format("{0}-th pages", m_counter);
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            PassStatePage2 page2 = new PassStatePage2 {Message = tbxInput.Text};
            NavigationService.Navigate(page2);
        }
    }
}
