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
using EntityLib;

namespace MVVM.Students
{
    /// <summary>
    /// Interaction logic for StudentTable.xaml
    /// </summary>
    public partial class StudentTable : Window
    {
        private ViewModel m_viewmodel;

        public StudentTable()
        {
            InitializeComponent();

            m_viewmodel = new ViewModel();

            this.DataContext = m_viewmodel;
        }

        private void btnInspect_Click(object sender, RoutedEventArgs e)
        {
            Student currentStudent = dgStudents.SelectedItem as Student;

            string msg = string.Format("Selected Index={0}\nSelected Student: {1}",
                dgStudents.SelectedIndex,
                currentStudent == null ? "None" : currentStudent.Name);

            MessageBox.Show(msg, "test", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void dgStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_viewmodel.OnSelectionChanged();
        }
    }
}
