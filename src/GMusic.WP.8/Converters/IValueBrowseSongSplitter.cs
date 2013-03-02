using System;
using System.Windows.Data;
using System.Text.RegularExpressions;


namespace GMusic.WP._8.Converters
{
	public class IValueBrowseSongSplitTitle : IValueConverter
	{
		public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return ((string)value).Split(new[] { "££$$%%$$££" }, StringSplitOptions.None)[0];
		}

		public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value;
		}
	}

	public class IValueBrowseSongSplitArtist : IValueConverter
	{
		public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return ((string) value).Split(new[] { "££$$%%$$££" }, StringSplitOptions.None)[1];
		}

		public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value;
		}
	}
}
