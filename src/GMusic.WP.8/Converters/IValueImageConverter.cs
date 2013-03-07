using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GMusic.WP._8.Converters
{
	public class IValueImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
                var output = (string)value;
                if (output.StartsWith("//"))
                    output = output.Insert(0, "https:");

				return new BitmapImage(new Uri(output));
			}
			catch
			{
				return new BitmapImage();
			}
		}

		public object ConvertBack(object value, Type targetType,
								  object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
