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
    /// Interaction logic for PassStatePage2.xaml
    /// </summary>
    public partial class PassStatePage2 : Page
    {
        public PassStatePage2()
        {
            InitializeComponent();
        }

        public string Message
        {
            get { return lblMessage.Content.ToString(); }
            set { lblMessage.Content = value; }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // this maybe better, but we want to try another way: this.NavigationService.GoBack();
            // note: navigate to a URI ('Relative' means relative path)
            NavigationService.Navigate(new Uri("PassStatePage1.xaml", UriKind.Relative));
        }
    }
}
