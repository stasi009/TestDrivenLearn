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

namespace _12.BindToList
{
    sealed class Researcher : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string m_name;
        public string Name
        {
            get { return m_name; }
            set
            {
                if (m_name == null || !m_name.Equals(value))
                {
                    m_name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string m_title;
        public string Title
        {
            get { return m_title; }
            set
            {
                if (m_title == null || !m_title.Equals(value))
                {
                    m_title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        private string m_organization;
        public string Organization
        {
            get { return m_organization; }
            set
            {
                if (m_organization == null ||  !m_organization.Equals(value))
                {
                    m_organization = value;
                    NotifyPropertyChanged("Organization");
                }
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Interaction logic for DataTemplate2.xaml
    /// </summary>
    public partial class ReferenceTemplate : Window
    {
        public ReferenceTemplate()
        {
            InitializeComponent();

            this.DataContext = new Researcher[]
            {
                new Researcher {Name = "Tom",Title = "Professor",Organization = "WSU"},
                new Researcher {Name = "Chuanlin",Title = "Doctor",Organization = "Tsinghua"},
                new Researcher {Name = "Mary",Title = "Associate",Organization = "NASA"},
                new Researcher {Name = "Dick",Title = "Engineer",Organization = "EPRI"},
            };
        }
    }
}
