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
		public SVNLog SvnLog { get; set; }
		public FileBrowser FileBrowser { get; set; }
		public string SvnServerPath { get; }
		public string SvnCheckoutPath { get; }

		public MainWindow()
		{
			SvnServerPath = @"D:\school\programmen\WPFTest\SVNServer";
			SvnCheckoutPath = @"D:\school\programmen\WPFTest\SVNCheckout";

			SvnLog = new SVNLog();

			InitializeComponent();
			this.DataContext = SvnLog;
			GoToFileBrowser();
		}

		private void RefreshButton_Click(object _sender, RoutedEventArgs _e)
		{
			SvnLog.RefreshLog();
		}

		private void NextWindow_Click(object _sender, RoutedEventArgs _e)
		{
			GoToFileBrowser();
		}

		private void GoToFileBrowser()
		{
			FileBrowser = new FileBrowser();
			Content = FileBrowser;
		}
	}
}
