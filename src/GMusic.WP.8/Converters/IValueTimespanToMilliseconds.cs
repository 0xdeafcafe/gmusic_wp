using System;
using System.Globalization;
using System.Windows.Data;

namespace GMusic.WP._8.Converters
{
    public class IValueTimespanToMilliseconds : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (TimeSpan)value;
            return string.Format("{0}", (int)time.TotalMilliseconds);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
