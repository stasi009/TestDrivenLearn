using System.Windows;
using System.ComponentModel;

using _11.DataBinding;

namespace Dialogs
{
    /// <summary>
    /// Interaction logic for PersonEditDlg.xaml
    /// </summary>
    public partial class PersonEditDlg : Window
    {
        private Person m_localCopy;

        public PersonEditDlg()
        {
            m_localCopy = new Person();

            InitializeComponent();

            // note: my intention to use the code below is to check whether in design mode, 
            // to set some fake data to be shown in the UI designer
            // however, it has no effect in the designer, maybe it only works in Blender
            // grid.DataContext = DesignerProperties.GetIsInDesignMode(this) ? new Person { Name = "test", Age = 10 } : person;

            grid.DataContext = m_localCopy;
        }

        public Person Person
        {
            get
            {
                return m_localCopy;
            }
            set
            {
                m_localCopy.CopyFrom(value);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
