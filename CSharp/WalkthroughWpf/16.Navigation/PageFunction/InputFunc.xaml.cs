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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _16.Navigation.PageFunction
{
    /// <summary>
    /// Interaction logic for OkOrCancelFunc.xaml
    /// </summary>
    public partial class InputFunc : PageFunction<String>
    {
        public InputFunc()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            OnReturn(new ReturnEventArgs<string>(tbxInput.Text));
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            OnReturn(null);
        }
    }
}
