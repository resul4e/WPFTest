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
	    public MenuItem Root { get; set; }
		public MainWindow MainWindow { get; set; }

        public FileBrowser()
        {
            InitializeComponent();

			//initialize the root folder of the repo
	        Root = new MenuItem() { Title = "SVNCheckout", Path = "D:\\school\\programmen\\WPFTest\\SVNCheckout\\" };
	        Root.Items.Add(new MenuItem() { Title = "{Dummy}", Path = "D:\\school\\programmen\\WPFTest\\SVNCheckout\\" });
	        trvMenu.Items.Add(Root);
        }

		//get a list of all of the directories and files inside of the given path
	    public List<MenuItem> GetDirectoryContent(string _path)
	    {
			List<MenuItem> directoryItems = new List<MenuItem>();

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
			    var commonDir = FileHelper.FindCommonPath("\\", new List<string>() { dir, _path });
			    var partDir = dir.Remove(0, commonDir.Length);
			    partDir = partDir.Remove(0, 1);
			
				//create an entry for the folder name
			    var newDirItem = new MenuItem() { Title = partDir };
			    newDirItem.Path = dir;

				//if the map doesn't contain anything don't add the dummy file
			    if (Directory.EnumerateFileSystemEntries(dir).Any())
			    {
				    newDirItem.Items.Add(new MenuItem() { Title = "{Dummy}", Path = dir });
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

	    private void trvMenuItem_Expanded(object _sender, RoutedEventArgs _e)
	    {
		    var tvi = (TreeViewItem) _e.OriginalSource;
		    var parentPath = ((MenuItem) tvi.Items[0]).Path;

		    tvi.ItemsSource = null;

			var directories = GetDirectoryContent(parentPath);

			foreach (var dir in directories)
			{
				tvi.Items.Add(dir);
			}
	    }

	    private void trvMenu_SelectionChanged(object _sender, RoutedPropertyChangedEventArgs<object> _e)
	    {
		    string path = ((MenuItem) _e.NewValue).Path;
		    MainWindow.SvnLog.Path = path;
	    }
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
