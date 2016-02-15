using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ListViewSimple.xaml
    /// </summary>
    public partial class ListViewSample : Window
    {
        private ObservableCollection<Student> m_students;

        public ListViewSample()
        {
            InitializeComponent();

            m_students = new ObservableCollection<Student>
                             {
                                 new Student{Id = 1,Name = "Dave",IsQualified = true,Notes = "Diligent"},
                                 new Student{Id = 2,Name = "Mary",IsQualified = false,Notes = "Nice"}
                             };
            this.DataContext = m_students;
        }
    }
}
