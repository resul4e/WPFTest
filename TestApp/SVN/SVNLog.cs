using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using SharpSvn;
using TestApp.Annotations;

namespace TestApp.SVN
{
	public class SVNLogData
	{
		public string Message { get; set; }
		public long Revision { get; set; } = -1;
		public DateTime Time { get; set; }
	}

	public class SVNLog : INotifyPropertyChanged
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

	    public string Path
	    {
		    get { return m_path;}
		    set { m_path = value;
			    RefreshLog();
		    }
	    } 
	    private string m_path = @"D:/school/programmen/WPFTest/SVNCheckout";


		public SVNLog()
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

		    Collection<SvnStatusEventArgs> svnStatusList;
		    client.GetStatus(Path, out svnStatusList);

		    if (svnStatusList.Count != 0)
		    {
			    if (Path == svnStatusList[0].Path && svnStatusList[0].LocalNodeStatus == SvnStatus.NotVersioned)
			    {
				    return;
			    }

		    }

		    client.Log(Path, (a, b) =>
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

	    ~SVNLog()
	    {
		    client.Dispose();
	    }
	}
}
