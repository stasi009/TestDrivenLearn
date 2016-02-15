using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using EntityLib;

namespace Controls
{
    /// <summary>
    /// Interaction logic for DataGridSample.xaml
    /// </summary>
    public partial class DataGridSample : Window
    {
        private ObservableCollection<Student> m_students;
        private ICollectionView m_collectView;

        public DataGridSample()
        {
            InitializeComponent();

            m_students = new ObservableCollection<Student>
            {
                new Student{Id = 1,Name = "Dave",Gender = Gender.Male, IsQualified = true,Notes = "Diligent"},
                new Student{Id = 2,Name = "Mary",Gender = Gender.Female, IsQualified = false,Notes = "Nice"},
                new Student{Id = 3,Name = "Tom",Gender = Gender.Male, IsQualified = false,Notes = "Bad"},
                new Student{Id = 4,Name = "Dick",Gender = Gender.Male,IsQualified = true,Notes = "Good"},
                new Student{Id = 5,Name = "Alice",Gender = Gender.Female,IsQualified = false,Notes = "Pretty"}
            };
            dgStudents.ItemsSource = m_students;
            m_collectView = CollectionViewSource.GetDefaultView(m_students);
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
