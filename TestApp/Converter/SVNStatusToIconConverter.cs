using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
			string imageSource = null;

			switch (status)
			{
				case SvnStatus.Incomplete:
				case SvnStatus.External:
				case SvnStatus.Obstructed:
				case SvnStatus.Ignored:
				case SvnStatus.Conflicted:
				case SvnStatus.Merged:
				case SvnStatus.Modified:
					imageSource = "pack://application:,,,/Media/Flaticon/pencil-edit-button16x16.png";
					break;
				case SvnStatus.Replaced:
				case SvnStatus.Deleted:
					break;
				case SvnStatus.Added:
					imageSource = "pack://application:,,,/Media/Flaticon/add16x16.png";
					break;
				case SvnStatus.Normal:
					imageSource = "pack://application:,,,/Media/Flaticon/checked16x16.png";
					break;
				case SvnStatus.NotVersioned:
					imageSource = "pack://application:,,,/Media/Flaticon/question16x16.png";
					break;
			}

			if(imageSource == null)
			{
				return null;
			}

			BitmapImage bi = new BitmapImage();
			bi.BeginInit();
			bi.UriSource = new Uri(imageSource, UriKind.Absolute);
			bi.EndInit();

			return bi ;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
