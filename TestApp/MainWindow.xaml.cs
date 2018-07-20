using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

namespace TestApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public SVNTest SvnTest { get; set; } = new SVNTest();

		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = SvnTest;
		}

		private void RefreshButton_Click(object sender, RoutedEventArgs e)
		{
			ListView.AlternationCount = 1;
			SvnTest.RefreshLog();
		}
	}

	public class SVNLogData
	{
		public string Message { get; set; }
		public long Revision { get; set; } = -1;
	}

	// ReSharper disable once InconsistentNaming
	public class SVNTest : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public ObservableCollection<SVNLogData> LogDataCollection
		{
			get
			{
				return m_logDataCollection;
			}
			set
			{
				m_logDataCollection = value;
				RaisePropertyChanged();
			}
		}
		private ObservableCollection<SVNLogData> m_logDataCollection = new ObservableCollection<SVNLogData>();

		private SvnClient client = new SvnClient();

		public SVNTest()
		{
			RefreshLog();
		}

		public void RefreshLog()
		{
			LogDataCollection.Clear();

			SvnUriTarget target = new SvnUriTarget(System.IO.Path.GetFullPath("D:/school/programmen/WPFTest/SVNServer"));

			SvnLogArgs args = new SvnLogArgs();
			args.Start = 0;
			args.Limit = 100;
			client.Log("D:\\school\\programmen\\WPFTest\\SVNCheckout", (a, b) =>
			{
				var logData = new SVNLogData()
				{
					Message = b.LogMessage,
					Revision = b.Revision
				};

				if (logData.Revision == 0)
				{
					logData.Message = "<initial commit>";
				}

				LogDataCollection.Add(logData);
				RaisePropertyChanged(nameof(LogDataCollection));
			});
		}

		~SVNTest()
		{
			client.Dispose();
		}

	}
}
