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
    /// Interaction logic for ObjectDataProvider.xaml
    /// </summary>
    public partial class ObjectDataProvider : Window
    {
        public ObjectDataProvider()
        {
            InitializeComponent();
        }

        private ICollectionView GetView(string resourceName)
        {
            DataSourceProvider provider = (DataSourceProvider)FindResource(resourceName);
            PersonCollection persons = (PersonCollection)provider.Data;
            return CollectionViewSource.GetDefaultView(persons);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = GetView("staticLoader");
            view.CircelMoveNext();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = GetView("instanceLoader");
            view.CircleMovePrevious();
        }
    }

    sealed class PeopleLoader
    {
        public static PersonCollection StaticLoad()
        {
            return Make("static", 10);
        }

        public PersonCollection InstanceLoad(int number)
        {
            return Make("instance", number);
        }

        private static PersonCollection Make(string prefix, int count)
        {
            PersonCollection persons = new PersonCollection();
            for (int index = 1; index <= count; ++index)
            {
                persons.Add(new Person
                                {
                                    Name = string.Format("{0}-{1}", prefix, index),
                                    Age = index * 10,
                                    SSN = (uint)index
                                });
            }
            return persons;
        }
    }
}
