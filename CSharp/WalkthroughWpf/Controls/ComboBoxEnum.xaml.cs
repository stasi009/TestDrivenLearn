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
using EntityLib;

namespace Controls
{
    /// <summary>
    /// Interaction logic for ComboBoxEnum.xaml
    /// </summary>
    public partial class ComboBoxEnum : Window,INotifyPropertyChanged
    {
        public sealed class Description
        {
            public string Roles { get; set; }
            public Gender Gender { get; set; }
        }
        private static Description[] m_descriptions;

        public static Description[] Descriptions
        {
            get
            {
                if (m_descriptions == null)
                    m_descriptions = new Description[] 
                    {
                        new Description{Roles = "Man,Boy,Husband",Gender = Gender.Male},
                        new Description{Roles = "Woman,Girl,Wife",Gender = Gender.Female}
                    };
                return m_descriptions;
            }
        }


        #region "observable properties"
        
        private Gender m_gender;
        public Gender Gender
        {
            get { return m_gender; }
            set
            {
                if (m_gender != value)
                {
                    m_gender = value;
                    NotifyPropertyChanged("Gender");
                }
            }
        }

        #endregion

        public ComboBoxEnum()
        {
            InitializeComponent();

            this.Gender = Gender.Female;
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, this.Gender.ToString(), "Information");
        }

        #region "implement INotifyPropertyChanged"

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
