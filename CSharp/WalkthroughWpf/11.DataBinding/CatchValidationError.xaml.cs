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

namespace _11.DataBinding
{
    /// <summary>
    /// Interaction logic for CatchValidationError.xaml
    /// </summary>
    public partial class CatchValidationError : Window
    {
        public CatchValidationError()
        {
            InitializeComponent();
        }

        private void OnAgeError(object sender, ValidationErrorEventArgs e)
        {
            MessageBox.Show(e.Error.ErrorContent.ToString(), "Age Error!!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
