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
		public string SVNServerPath { get; private set; }
		public string SVNCheckoutPath { get; private set; }

		public MainWindow()
		{
			SVNServerPath = @"D:\school\programmen\WPFTest\SVNServer";
			SVNCheckoutPath = @"D:\school\programmen\WPFTest\SVNCheckout";

			SvnLog = new SVNLog(this);

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
			FileBrowser = new FileBrowser(this);
			Content = FileBrowser;
			FileBrowser.MainWindow = this;
		}
	}
}
