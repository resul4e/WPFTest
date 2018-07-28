using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SharpSvn;
using TestApp.Annotations;

namespace TestApp.SVN
{
	/// <summary>
	/// Stores all of the data the UI needs to show the log
	/// </summary>
	public class SVNLogData
	{
		public string Message { get; set; }
		public long Revision { get; set; } = -1;
		public DateTime Time { get; set; }
	}

	/// <summary>
	/// Stores and retrieves svn log data.
	/// </summary>
	public class SVNLog : INotifyPropertyChanged
    {
	    #region Public Properties
		
	    /// <summary>
		/// List of SVNLogDatas that is shown in the UI
		/// </summary>
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

		/// <summary>
		/// Root folder where all of the svn files are located
		/// </summary>
	    public string SvnCheckoutPath
		{
		    get { return m_path;}
		    set { m_path = value;
			    RefreshLog();
		    }
	    }

	    public SvnClient Client
	    {
		    get
		    {
			    return m_client;

		    }
	    }

		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// Populate path and refresh log
		/// </summary>
		public SVNLog()
		{
			SvnCheckoutPath = ((MainWindow)Application.Current.MainWindow)?.SvnCheckoutPath;

			RefreshLog();
		}

	    ~SVNLog()
	    {
		    m_client.Dispose();
	    }

		#endregion

		/// <summary>
		/// Refreshes the log, taking in account the path currently set.
		/// </summary>
		public void RefreshLog()
	    {
		    LogDataCollection.Clear();

		    SvnLogArgs args = new SvnLogArgs();
		    args.Start = args.End;
		    args.Limit = 100;

		    m_client.GetStatus(SvnCheckoutPath, out Collection<SvnStatusEventArgs> svnStatusList);

		    if (svnStatusList.Count != 0)
		    {
			    if (SvnCheckoutPath == svnStatusList[0].Path && svnStatusList[0].LocalNodeStatus == SvnStatus.NotVersioned)
			    {
				    return;
			    }
		    }

		    m_client.Log(SvnCheckoutPath, args, (a, b) =>
		    {
			    var logData = new SVNLogData()
			    {
				    Message = b.LogMessage,
				    Revision = b.Revision,
				    Time = b.Time
			    };

			    if (logData.Revision == 0 && string.IsNullOrEmpty(logData.Message))
			    {
				    logData.Message = "<initial commit>";
			    }

			    LogDataCollection.Add(logData);
			    RaisePropertyChanged(nameof(LogDataCollection));
		    });
	    }

		#region IPropertyChanged

	    public event PropertyChangedEventHandler PropertyChanged;

	    [NotifyPropertyChangedInvocator]
	    protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
	    {
		    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	    }

		#endregion

		#region Private Properties

	    private string m_path;
	    private ObservableCollection<SVNLogData> m_logDataCollection = new ObservableCollection<SVNLogData>();
	    private SvnClient m_client = new SvnClient();

		#endregion
	}
}
