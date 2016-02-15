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
    /// <summary>
    /// Interaction logic for SimpleList.xaml
    /// </summary>
    public partial class SimpleList : Window
    {
        private readonly ICollectionView m_viewTeam;

        public SimpleList()
        {
            InitializeComponent();

            m_viewTeam = Helper.MakeTeamView(6);

            gridTeam.DataContext = m_viewTeam;
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
