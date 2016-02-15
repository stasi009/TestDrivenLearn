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
using System.Diagnostics;
using _11.DataBinding;

namespace _12.BindToList
{
    /// <summary>
    /// Interaction logic for SelectValue.xaml
    /// </summary>
    public partial class SelectValue : Window
    {
        public SelectValue()
        {
            InitializeComponent();
        }

        // chekatodo: !!!!!!!!! there is a problem about the following callback
        // the problem is that both manual selection change and the auto selection change
        // caused by wpf binding mechanism will both fire following callback
        // everytime, when this callback is fired by manual selection change, everything works fine
        // however, as long as it is fired by binding mechanism, it seems that the selected item
        // is always referring to the previous selected one, other than the current one
        // !!!!!!!!!!!!! so maybe the only method to refresh the label is still using data binding
        private void cbxPersons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Person selectedPerson = (Person)cbxPersons.SelectedItem;
            lblSSN.Content = string.Format("SSN: {0}", cbxPersons.SelectedValue);
        }

        private void btnInspect_Click(object sender, RoutedEventArgs e)
        {
            PersonCollection persons = (PersonCollection)FindResource("persons");
            int index = 0;
            foreach (var person in persons)
            {
                ++index;
                Console.WriteLine("{0}-th: SSN={1},Name={2}", index, person.SSN, person.Name);
            }
            Console.WriteLine();
        }

        private void lbxPersons_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Debug.Assert(lbxPersons.SelectedIndex >= 0);

            uint ssn = (uint)lbxPersons.SelectedValue;
            MessageBox.Show(string.Format("SSN:{0}", ssn),
                "SSN",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
