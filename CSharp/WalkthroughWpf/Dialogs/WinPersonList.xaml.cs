
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

using _11.DataBinding;

namespace Dialogs
{
    /// <summary>
    /// Interaction logic for WinPersonList.xaml
    /// </summary>
    public partial class WinPersonList : Window
    {
        private readonly ObservableCollection<Person> m_personCollection;

        public WinPersonList()
        {
            InitializeComponent();

            m_personCollection = new ObservableCollection<Person>(from num in Enumerable.Range(1, 8)
                                                                  select new Person
                                                                             {
                                                                                 Name = string.Format("person{0}", num),
                                                                                 Age = num * 10
                                                                             });
            lbxPersons.DataContext = m_personCollection;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Person currentPerson = (Person)lbxPersons.SelectedItem;

            PersonEditDlg dlg = new PersonEditDlg
                                    {
                                        Owner = this,
                                        Person = currentPerson
                                    };
            if (dlg.ShowDialog() == true)
            {
                currentPerson.CopyFrom(dlg.Person);
            }
        }

        private void lbxPersons_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            btnEdit_Click(sender,e);
        }
    }
}
