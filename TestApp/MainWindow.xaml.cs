using System.Windows;
using TestApp.FileManagement;
using TestApp.SVN;

namespace TestApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Public Properties
		/// <summary>
		/// stores and retrieves svn log data.
		/// </summary>
		public SVNLog SvnLog { get; set; }
		
		/// <summary>
		/// Holds File tree with the <see cref="SvnCheckoutPath"/> as the root.
		/// </summary>
		public FileBrowser FileBrowser { get; set; }
		
		/// <summary>
		/// The path to the svn server.
		/// </summary>
		public string SvnServerPath { get; }

		/// <summary>
		/// The path to the checkout folder.
		/// </summary>
		public string SvnCheckoutPath { get; }
		#endregion

		#region Ctor/Dtor

		public MainWindow()
		{
			SvnServerPath = @"D:\school\programmen\WPFTest\SVNServer";
			SvnCheckoutPath = @"D:\school\programmen\WPFTest\SVNCheckout";

			SvnLog = new SVNLog();

			InitializeComponent();
			DataContext = SvnLog;
			GoToFileBrowser();
		}

		#endregion

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
