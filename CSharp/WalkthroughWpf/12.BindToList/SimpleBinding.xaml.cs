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
    /// Interaction logic for SimpleBinding.xaml
    /// </summary>
    public partial class SimpleBinding : Window
    {
        private ICollectionView m_viewTeam;

        public SimpleBinding()
        {
            InitializeComponent();

            // ------------------------ initialize members
            m_viewTeam = Helper.MakeTeamView(5);

            // ------------------------ bind
            // note: assign the DataContext to a list of data, because the control can only show
            // a single element, so it will only show the current element
            // and by default, the current element of a list is the first one
            gridPerson.DataContext = m_viewTeam;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            m_viewTeam.CircelMoveNext();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            m_viewTeam.CircleMovePrevious();
        }
    }
}
