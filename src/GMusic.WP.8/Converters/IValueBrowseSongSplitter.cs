using System;
using System.Windows.Data;


namespace GMusic.WP._8.Converters
{
	public class IValueBrowseSongSplitTitle : IValueConverter
	{
		public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (((string)value).ToLower().Contains("(i can"))
			{
				
			}

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
