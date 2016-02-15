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

namespace _06.ResourceStyle.Resource
{
    /// <summary>
    /// Interaction logic for ReferNonResource.xaml
    /// </summary>
    public partial class ReferNonResource : Window
    {
        #region "constant definition"

        public static readonly SolidColorBrush Brush = new SolidColorBrush(Colors.SteelBlue);

        #endregion

        public ReferNonResource()
        {
            InitializeComponent();
        }
    }
}
