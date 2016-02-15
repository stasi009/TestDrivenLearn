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
    /// Interaction logic for SimpleDataBinding.xaml
    /// </summary>
    public partial class SimpleDataBinding : Window
    {
        public SimpleDataBinding()
        {
            InitializeComponent();
        }

        private void btnYouger_Clicked(object sender, RoutedEventArgs e)
        {
            Person author = (Person)FindResource("author");
            --author.Age;
        }
    }
}
