using System;
using System.Globalization;
using System.Windows.Data;

namespace GMusic.WP._8.Converters
{
    public class IValueTimespanToFriendly : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (TimeSpan) value;
            return string.Format("{0}:{1:00}", time.Minutes, time.Seconds);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
