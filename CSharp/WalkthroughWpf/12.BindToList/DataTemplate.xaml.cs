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
    /// Interaction logic for DataTemplate.xaml
    /// </summary>
    public partial class DataTemplate : Window
    {
        private ICollectionView m_personsView;

        public DataTemplate()
        {
            InitializeComponent();

            // initialize peoples
            PersonCollection persons = new PersonCollection();

            for (int index = 1; index <= 10; ++index)
            {
                persons.Add(new Person
                                  {
                                      Name = string.Format("Person{0}", index),
                                      SSN = (uint)index,
                                      Age = index * 10
                                  });
            }
            m_personsView = CollectionViewSource.GetDefaultView(persons);
            gridTeam.DataContext = m_personsView;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            m_personsView.CircelMoveNext();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            m_personsView.CircleMovePrevious();
        }
    }
}
