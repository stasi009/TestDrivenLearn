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

namespace _06.ResourceStyle.Style
{
    /// <summary>
    /// Interaction logic for SimpleStyle.xaml
    /// </summary>
    public partial class SimpleStyle : Window
    {
        public SimpleStyle()
        {
            InitializeComponent();

            // note: setting the style programmingly can solve the problem that 
            // "updating the style of multiple controls simultaneously"
            btnWait.Style =(System.Windows.Style) FindResource("BtnStyle");
        }
    }
}
