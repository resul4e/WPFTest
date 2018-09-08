using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using SharpSvn;
using TestApp.Helpers;
using Path = System.IO.Path;

namespace TestApp.FileManagement
{
	/// <summary>
	/// Interaction logic for FileBrowser.xaml
	/// </summary>
	public partial class FileBrowser : Page
    {
		#region Public Properties

		/// <summary>
		/// Root folder the user has selected.
		/// </summary>
		public FileBrowserData Root { get; set; }

		public MainWindow MainWindow { get; set; }

		#endregion

		#region Ctor

		public FileBrowser()
		{
			MainWindow = Application.Current.MainWindow as MainWindow;

			InitializeComponent();

			//initialize the root folder of the repo
			Root = new FileBrowserData() { Title = Path.GetFileNameWithoutExtension(MainWindow.SvnCheckoutPath), Path = MainWindow.SvnCheckoutPath };
			Root.Items.Add(new FileBrowserData() { Title = m_dummyName, Path = MainWindow.SvnCheckoutPath });
			trvMenu.Items.Add(Root);

			m_listViewSortAdorner = new SortAdorner(ListView, ListSortDirection.Descending);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Get a list of all of the directories and files inside of the given path.
		/// </summary>
		/// <param name="_path">The DirectoryPath you want the content for.</param>
		/// <returns>A list of MenuItems, if the menuItem is another directory it is filled with a dummy item, otherwise its content remains empty</returns>
		public List<FileBrowserData> GetDirectoryContent(string _path)
	    {
			List<FileBrowserData> directoryItems = new List<FileBrowserData>();

		    if (!Directory.Exists(_path))
		    {
			    return directoryItems;
		    }

		    var allDirs = System.IO.Directory.EnumerateDirectories(_path, "*",
			    SearchOption.TopDirectoryOnly);
			
			//get all the folders in the current folder
		    foreach (var dir in allDirs)
		    {
				//check if the file is hidden, if so, don't show it.
			    DirectoryInfo info = new DirectoryInfo(dir);
			    if ((info.Attributes & FileAttributes.Hidden) != 0)
			    {
				    continue;
			    }
			
				//get the folder name
			    var commonDir = FileHelper.FindCommonPath(Path.DirectorySeparatorChar.ToString(), new List<string> { dir, _path });
			    var partDir = dir.Remove(0, commonDir.Length);
			    partDir = partDir.Remove(0, 1);
			
				//create an entry for the folder name
			    var newDirItem = new FileBrowserData() { Title = partDir };
			    newDirItem.Path = dir;

				//if the map doesn't contain anything don't add the dummy file
			    if (Directory.EnumerateFileSystemEntries(dir).Any())
			    {
				    newDirItem.Items.Add(new FileBrowserData() { Title = m_dummyName, Path = dir });
				}

			    directoryItems.Add(newDirItem);
			}

		    //get all of the files in the current folder 
		    var files = Directory.GetFiles(_path);
		    foreach (var file in files)
		    {
			    var fileName = Path.GetFileName(file);
			    directoryItems.Add(new FileBrowserData() { Title = fileName, Path = file });
		    }

			return directoryItems;
	    }

		#endregion

		#region Private Methods

		private void ResetSortAdorner()
		{
			if (m_sortedColumn != null)
			{
				AdornerLayer.GetAdornerLayer(m_sortedColumn).Remove(m_listViewSortAdorner);
				ListView.Items.SortDescriptions.Clear();
			}
		}

		#endregion

		#region Events

		private void trvMenuItem_Expanded(object _sender, RoutedEventArgs _e)
	    {
		    var tvi = (TreeViewItem)_e.OriginalSource;

		    if (!tvi.HasItems)
		    {
			    return;
		    }
		   
			//if we already populated the list we don't need to do it again.
		    if (((FileBrowserData)tvi.Items[0]).Title != m_dummyName)
		    {
			    return;
		    }

		    var parentPath = ((FileBrowserData)tvi.Items[0]).Path;

		    tvi.ItemsSource = null;

		    var directories = GetDirectoryContent(parentPath);

		    SvnStatusArgs args = new SvnStatusArgs()
		    {
			    Depth = SvnDepth.Empty,
			    RetrieveRemoteStatus = true,
			    RetrieveAllEntries = true
			};


		    using (SvnClient client = new SvnClient())
		    {
			    foreach (var dir in directories)
			    {
				    client.GetStatus(dir.Path, args, out var statusList);

				    if (statusList.Count != 0)
				    {
					    dir.SvnStatus = statusList[0].LocalNodeStatus;
					}
				    else
				    {
					    dir.SvnStatus = SvnStatus.NotVersioned;
				    }

					tvi.Items.Add(dir);
			    }
		    }
	    }

	    private void GrVHeader_Clicked(object _sender, RoutedEventArgs _e)
	    {
			var column = (GridViewColumnHeader)_sender;
			string sortBy = column.Tag.ToString();

			ResetSortAdorner();

			ListSortDirection newDir = ListSortDirection.Ascending;
			if (m_sortedColumn == column && m_listViewSortAdorner.SortDirection == newDir)
			{
				newDir = ListSortDirection.Descending;
			}

			m_sortedColumn = column;
			m_listViewSortAdorner = new SortAdorner(m_sortedColumn, newDir);
			AdornerLayer.GetAdornerLayer(m_sortedColumn).Add(m_listViewSortAdorner);
			ListView.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
		}

		private void GrVHeader_RightClicked(object _sender, MouseButtonEventArgs _e)
		{
			ResetSortAdorner();
		}

		private void trvMenu_SelectionChanged(object _sender, RoutedPropertyChangedEventArgs<object> _e)
	    {
		    string path = ((FileBrowserData) _e.NewValue).Path;
		    MainWindow.SvnLog.SvnSelectedPath = path;
	    }

	    #endregion

		#region Private Properties

		private string m_dummyName = "{Dummy}";
		private GridViewColumnHeader m_sortedColumn;
	    private SortAdorner m_listViewSortAdorner;

	    #endregion
    }

	public class FileBrowserData
	{
		public FileBrowserData()
		{
			this.Items = new ObservableCollection<FileBrowserData>();
		}

		public string Title { get; set; }
		public string Path { get; set; }
		public SvnStatus SvnStatus { get; set; }

		public ObservableCollection<FileBrowserData> Items { get; set; }
	}
}
