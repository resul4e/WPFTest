using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpSvn;
using System.IO;
using System.Runtime.CompilerServices;
using TestApp.Annotations;
using TestApp.SVN;

namespace TestApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public SVNLog SvnLog { get; set; } = new SVNLog();
		public FileBrowser FileBrowser { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = SvnLog;
			GoToFileBrowser();
		}

		private void RefreshButton_Click(object sender, RoutedEventArgs e)
		{
			SvnLog.RefreshLog();
		}

		private void NextWindow_Click(object sender, RoutedEventArgs e)
		{
			GoToFileBrowser();
		}

		private void GoToFileBrowser()
		{
			FileBrowser = new FileBrowser();
			this.Content = FileBrowser;
			FileBrowser.MainWindow = this;
		}
	}

	
	public class CConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var dateTime = (DateTime)value;
			
			return dateTime.GetDateTimeFormats()[90];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
