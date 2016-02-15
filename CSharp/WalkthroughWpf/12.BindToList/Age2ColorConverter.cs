using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace _12.BindToList
{
    [ValueConversion(typeof(int), typeof(Brush))]
    public sealed class Age2ColorConverter : IValueConverter
    {
        private static readonly Type m_expectedTargetType = typeof(Brush);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != m_expectedTargetType)
                return null;
            else
            {
                int number = (int)value;

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
}
