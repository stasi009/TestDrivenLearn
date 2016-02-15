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

namespace _11.DataBinding
{
    /// <summary>
    /// Interaction logic for TwowayBinding.xaml
    /// </summary>
    public partial class TwowayBinding : Window
    {
        public TwowayBinding()
        {
            InitializeComponent();
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            Person author = this.Author;
            MessageBox.Show(string.Format("{0} is {1}", author.Name, author.Age), 
                "Information", 
                MessageBoxButton.OK,
                MessageBoxImage.Asterisk);
        }

        private void btnYounger_Click(object sender, RoutedEventArgs e)
        {
            -- Author.Age;
        }

        private Person Author
        {
            get { return (Person)FindResource("author"); }
        }
    }
}
