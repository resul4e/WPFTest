using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
using TestApp.Helpers;
using Path = System.IO.Path;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for FileBrowser.xaml
    /// </summary>
    public partial class FileBrowser : Page
    {
		/// <summary>
		/// Root folder of 
		/// </summary>
	    public MenuItem Root { get; set; }
		public MainWindow MainWindow { get; set; }

        public FileBrowser()
        {
	        MainWindow = Application.Current.MainWindow as MainWindow;

            InitializeComponent();

			//initialize the root folder of the repo
	        Root = new MenuItem() { Title = Path.GetFileNameWithoutExtension(MainWindow.SvnCheckoutPath), Path = MainWindow.SvnCheckoutPath };
	        Root.Items.Add(new MenuItem() { Title = m_dummyName, Path = MainWindow.SvnCheckoutPath });
	        trvMenu.Items.Add(Root);
        }

		/// <summary>
		/// Get a list of all of the directories and files inside of the given path
		/// </summary>
		/// <param name="_path">The DirectoryPath you want the content for.</param>
		/// <returns>A list of MenuItems, if the menuItem is another directory it is filled with a dummy item, otherwise its content remains empty</returns>
		public List<MenuItem> GetDirectoryContent(string _path)
	    {
			List<MenuItem> directoryItems = new List<MenuItem>();

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
			    var commonDir = FileHelper.FindCommonPath(Path.DirectorySeparatorChar.ToString(), new List<string>() { dir, _path });
			    var partDir = dir.Remove(0, commonDir.Length);
			    partDir = partDir.Remove(0, 1);
			
				//create an entry for the folder name
			    var newDirItem = new MenuItem() { Title = partDir };
			    newDirItem.Path = dir;

				//if the map doesn't contain anything don't add the dummy file
			    if (Directory.EnumerateFileSystemEntries(dir).Any())
			    {
				    newDirItem.Items.Add(new MenuItem() { Title = m_dummyName, Path = dir });
				}

			    directoryItems.Add(newDirItem);
			}

		    //get all of the files in the current folder 
		    var files = Directory.GetFiles(_path);
		    foreach (var file in files)
		    {
			    var fileName = Path.GetFileName(file);
			    directoryItems.Add(new MenuItem() { Title = fileName, Path = file });
		    }

			return directoryItems;
	    }

	    #region Events

		private void trvMenuItem_Expanded(object _sender, RoutedEventArgs _e)
	    {
		    var tvi = (TreeViewItem) _e.OriginalSource;
			//if we already populated the list we don't need to do it again.
		    if (((MenuItem)tvi.Items[0]).Title != m_dummyName)
		    {
			    return;
		    }

		    var parentPath = ((MenuItem) tvi.Items[0]).Path;

		    tvi.ItemsSource = null;

			var directories = GetDirectoryContent(parentPath);

			foreach (var dir in directories)
			{
				tvi.Items.Add(dir);
			}
	    }

	    private void grvHeader_Clicked(object _sender, RoutedEventArgs _e)
	    {
		    throw new NotImplementedException();
	    }

		private void trvMenu_SelectionChanged(object _sender, RoutedPropertyChangedEventArgs<object> _e)
	    {
		    string path = ((MenuItem) _e.NewValue).Path;
		    MainWindow.SvnLog.SvnCheckoutPath = path;
	    }

	    #endregion

		#region Private Properties

		private string m_dummyName = "{Dummy}";

	    #endregion
    }

	public class MenuItem
	{
		public MenuItem()
		{
			this.Items = new ObservableCollection<MenuItem>();
		}

		public string Title { get; set; }
		public string Path { get; set; }

		public ObservableCollection<MenuItem> Items { get; set; }
	}
}
