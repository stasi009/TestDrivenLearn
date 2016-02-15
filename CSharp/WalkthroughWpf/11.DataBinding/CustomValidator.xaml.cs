using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for CustomValidator.xaml
    /// </summary>
    public partial class CustomValidator : Window
    {
        public CustomValidator()
        {
            InitializeComponent();
        }

        private void tbxAge_Error(object sender, ValidationErrorEventArgs e)
        {
            MessageBox.Show(e.Error.ErrorContent.ToString(), "Age Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // note: it is rather interesting that if you want code generated successfully by IDE
    // for you, the class which will hold the auto-generated code must be the first
    // class in that code-behind file
    public sealed class AgeValidator : ValidationRule
    {
        private int m_min;
        private int m_max;

        public int Min
        {
            get { return m_min; }
            set { m_min = value; }
        }

        public int Max
        {
            get { return m_max; }
            set { m_max = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int age;
            if (!int.TryParse((string)value, out age))
                return new ValidationResult(false, "invalid number format");

            if (age > m_max || age < m_min)
                return new ValidationResult(false, "age out of range");

            return ValidationResult.ValidResult;
        }
    }
}
