using System;
using System.Collections.Generic;
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
	public partial class MainWindow : Window , INotifyPropertyChanged
	{
		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public List<string> LogMessages
		{
			get
			{
				return m_logMessages;
			}
			set
			{
				m_logMessages = value;
				OnPropertyChanged();
			}
		} 
		private List<string> m_logMessages = new List<string>();

		public MainWindow()
		{

			InitializeComponent();

			using (SvnClient client = new SvnClient())
			{

				SvnUriTarget target = new SvnUriTarget(System.IO.Path.GetFullPath("D:/school/programmen/WPFTest/SVNServer"));

				SvnLogArgs args = new SvnLogArgs();
				args.Start = 0;
				args.Limit = 100;
				client.Log("D:\\school\\programmen\\WPFTest\\SVNCheckout", (a, b) => { LogMessages.Add(b.LogMessage); OnPropertyChanged(nameof(LogMessages)); });
			}
		}
	}
}
