using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TestApp.Converter
{
	public class DateTimeToStringConverter : IValueConverter
	{
		public static string DateTimeFormat { get; set; } = "dd-MM-yyyy HH:mm:ss";

		public object Convert(object _value, Type _targetType, object _parameter, CultureInfo _culture)
		{
			var dateTime = (DateTime) _value;

			return dateTime.ToString(DateTimeFormat);
		}

		public object ConvertBack(object _value, Type _targetType, object _parameter, CultureInfo _culture)
		{
			throw new NotImplementedException();
		}
	}
}
