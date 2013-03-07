using System;
using System.Globalization;
using System.Windows.Data;

namespace GMusic.WP._8.Converters
{
    public class IValueLazyGroupDescriptor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = value.ToString()[0];
            return (Char.IsLower(input));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
