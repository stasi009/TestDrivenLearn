using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using _11.DataBinding;

namespace _12.BindToList
{
    /// <summary>
    /// Interaction logic for AddToList.xaml
    /// </summary>
    public partial class ChangeList : Window
    {
        public ChangeList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// note: this method is only for demonstration, you cannot use the same text box
        /// for both edit the current one and insert the new one
        /// because when the current text boxes loses focus, it will automatically update 
        /// the current one, and then after you press "Add", actually 
        /// the current one has already been modified into the same content as the newly added one
        /// </summary>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Person newOne = new Person
                                {
                                    Name = tbxName.Text,
                                    Age = int.Parse(tbxAge.Text)
                                };
            this.Persons.Add(newOne);
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            Person deleted = (Person)lbxPersons.SelectedItem;
            this.Persons.Remove(deleted);
        }

        private void BtnSortByName_OnClick(object sender, RoutedEventArgs e)
        {
            ICollectionView view = this.View;
            // note: although the changes we made is on the view itself, but the data source
            // isn't the view, so it is clear that all the changes made through view
            // will eventually take effect in the background, real data source
            if (view.SortDescriptions.Count == 0)
            {
                // adding order matters, which decides which sort option is the priority
                // and which option is the secondary
                view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                view.SortDescriptions.Add(new SortDescription("Age", ListSortDirection.Descending));
            }
            else
            {
                view.SortDescriptions.Clear();
            }
        }

        private void BtnSortByAge_OnClick(object sender, RoutedEventArgs e)
        {
            ICollectionView view = this.View;
            if (view.SortDescriptions.Count == 0)
            {
                view.SortDescriptions.Add(new SortDescription("Age", ListSortDirection.Descending));
            }
            else
            {
                view.SortDescriptions.Clear();
            }
        }

        private void btnFilterAge_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = this.View;
            if (view.Filter == null)
            {
                view.Filter = obj => ((Person) obj).Age > 50;
            }
            else
            {
                view.Filter = null;
            }
        }

        private PersonCollection Persons
        {
            get { return (PersonCollection)FindResource("persons"); }
        }

        private ICollectionView View
        {
            get { return CollectionViewSource.GetDefaultView(this.Persons); }
        }
    }
}
