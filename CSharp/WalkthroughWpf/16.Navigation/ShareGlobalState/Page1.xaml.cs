using System;
using System.Collections;
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

namespace _16.Navigation.ShareGlobalState
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        private static int m_counter = 0;

        public Page1()
        {
            InitializeComponent();

            ++m_counter;
            lblCounter.Content = string.Format("totally, {0} pages have been constructed",m_counter);
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            // note: store data into application level cache
            IDictionary dict = Application.Current.Properties;
            dict["message"] = tbxInput.Text;

            // note: navigate to relative URI, BUT, this relative URI is not based on current page
            // but based on the base working directory
            NavigationService.Navigate(new Uri("ShareGlobalState/Page2.xaml", UriKind.Relative));
        }
    }
}
