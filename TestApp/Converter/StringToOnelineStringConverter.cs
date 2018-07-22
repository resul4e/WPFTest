using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TestApp.Converter
{
	public class StringToOnelineStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}

			var oldString = value as string ?? (string)value;
			if (string.IsNullOrEmpty(oldString))
			{
				return null;
			}

			return oldString.Replace(Environment.NewLine, " ");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
