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

	    public string SvnSelectedPath { get; set; }

		#endregion

		#region Ctor/Dtor

		/// <summary>
		/// Populate path and refresh log
		/// </summary>
		public SVNLog()
		{
			SvnCheckoutPath = ((MainWindow)Application.Current.MainWindow)?.SvnCheckoutPath;
			SvnSelectedPath = SvnCheckoutPath;

			RefreshLog();
		}

		#endregion

		/// <summary>
		/// Refreshes the log, taking in account the path currently set.
		/// </summary>
		public void RefreshLog()
	    {
		    if (string.IsNullOrEmpty(SvnSelectedPath) && !string.IsNullOrEmpty(SvnCheckoutPath))
		    {
			    SvnSelectedPath = SvnCheckoutPath;

		    }
		    else if(string.IsNullOrEmpty(SvnCheckoutPath) && string.IsNullOrEmpty(SvnSelectedPath))
		    {
			    throw new Exception("Both " + nameof(SvnSelectedPath) + " and" + nameof(SvnCheckoutPath) + "are empty!");
		    }

		    LogDataCollection.Clear();

		    SvnLogArgs args = new SvnLogArgs();
		    args.Start = args.End;
		    args.Limit = 100;


		    using (SvnClient client = new SvnClient())
		    {
			    client.GetStatus(SvnSelectedPath, out Collection<SvnStatusEventArgs> svnStatusList);

			    if (svnStatusList.Count != 0)
			    {
				    foreach (var status in svnStatusList)
				    {
					    if (SvnSelectedPath == status.Path && status.LocalNodeStatus == SvnStatus.NotVersioned)
					    {
						    return;
					    }
					}

			    }

			    client.Log(SvnSelectedPath, args, (_obj, _logArgs) =>
			    {
				    var logData = new SVNLogData()
				    {
					    Message = _logArgs.LogMessage,
					    Revision = _logArgs.Revision,
					    Time = _logArgs.Time
				    };

				    if (logData.Revision == 0 && string.IsNullOrEmpty(logData.Message))
				    {
					    logData.Message = "<initial commit>";
				    }

				    LogDataCollection.Add(logData);
				    RaisePropertyChanged(nameof(LogDataCollection));
			    });
		    }
	    }

		#region IPropertyChanged

	    public event PropertyChangedEventHandler PropertyChanged;

	    [NotifyPropertyChangedInvocator]
	    protected virtual void RaisePropertyChanged([CallerMemberName] string _propertyName = null)
	    {
		    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_propertyName));
	    }

		#endregion

		#region Private Properties

	    private string m_path;
	    private ObservableCollection<SVNLogData> m_logDataCollection = new ObservableCollection<SVNLogData>();

		#endregion
	}
}
