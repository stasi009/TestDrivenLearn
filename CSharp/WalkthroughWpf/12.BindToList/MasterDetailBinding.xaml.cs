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
using _11.DataBinding;

namespace _12.BindToList
{
    /// <summary>
    /// Interaction logic for MasterDetailBinding.xaml
    /// </summary>
    public partial class MasterDetailBinding : Window
    {
        private ObservableCollection<Family> m_familes;

        public MasterDetailBinding()
        {
            InitializeComponent();

            m_familes = Helper.MakeFamiles();

            grid.DataContext = m_familes;
        }
    }
}
