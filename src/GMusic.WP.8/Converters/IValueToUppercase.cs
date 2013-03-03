﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace GMusic.WP._8.Converters
{
	public class IValueToUppercase : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((string) value).ToUpper();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}