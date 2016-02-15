using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace _11.DataBinding
{
    [ValueConversion(typeof(double), typeof(Brush))]
    public sealed class Value2BrushConverter : IValueConverter
    {
        private static readonly Type m_expectedTargetType = typeof(Brush);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != m_expectedTargetType)
                return null;
            else
            {
                double number = (double)value;

                if (number < 33)
                    return Brushes.Green;
                else if (number >= 33 && number < 66)
                    return Brushes.Blue;
                else
                    return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Interaction logic for Converter.xaml
    /// </summary>
    public partial class Converter : Window
    {
        public Converter()
        {
            InitializeComponent();
        }
    }
}
