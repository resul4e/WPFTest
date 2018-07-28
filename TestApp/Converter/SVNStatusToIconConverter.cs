using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using SharpSvn;

namespace TestApp.Converter
{
	class SVNStatusToIconConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is SvnStatus))
			{
				return null;
			}

			SvnStatus status = (SvnStatus)value;

			switch (status)
			{
				case SvnStatus.Incomplete:
					return null;
				case SvnStatus.External:
					return null;
				case SvnStatus.Obstructed:
					return null;
				case SvnStatus.Ignored:
					return null;
				case SvnStatus.Conflicted:
					return null;
				case SvnStatus.Merged:
					return null;
				case SvnStatus.Modified:
					return null;
				case SvnStatus.Replaced:
					return null;
				case SvnStatus.Deleted:
					return null;
				case SvnStatus.Added:
					return "Media/FontAwesome/add-square-button.png";
				case SvnStatus.Normal:
					return "Media/FontAwesome/check.png";
				case SvnStatus.NotVersioned:
					return "Media/FontAwesome/check-box-empty.png";
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
