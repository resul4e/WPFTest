using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Path = System.IO.Path;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for FileBrowser.xaml
    /// </summary>
    public partial class FileBrowser : Page
    {
	    public MenuItem Root { get; set; } = new MenuItem();

        public FileBrowser()
        {
            InitializeComponent();

	        MenuItem root = new MenuItem() { Title = "SVNCheckout" };
	        var allDirs = System.IO.Directory.EnumerateDirectories("D:\\school\\programmen\\WPFTest\\SVNCheckout\\", "*",
		        SearchOption.TopDirectoryOnly);

	        foreach (var dir in allDirs)
	        {
				DirectoryInfo info = new DirectoryInfo(dir);
		        if ((info.Attributes & FileAttributes.Hidden) != 0)
		        {
					continue;
		        }


		        var commonDir = FindCommonPath("\\", new List<string>() { dir, "D:\\school\\programmen\\WPFTest\\SVNCheckout\\" });
		        var partDir = dir.Remove(0, commonDir.Length);
		        partDir = partDir.Remove(0, 1);

				var newDirItem = new MenuItem() {Title = partDir};


				var files = Directory.GetFiles(dir);
		        foreach (var file in files)
		        {
			        newDirItem.Items.Add(new MenuItem() {Title = file});

		        }


				root.Items.Add(newDirItem);
			}

			trvMenu.Items.Add(root);
		}

	    public static string FindCommonPath(string Separator, List<string> Paths)
	    {
		    string CommonPath = String.Empty;
		    List<string> SeparatedPath = Paths
			    .First(str => str.Length == Paths.Max(st2 => st2.Length))
			    .Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries)
			    .ToList();

		    foreach (string PathSegment in SeparatedPath.AsEnumerable())
		    {
			    if (CommonPath.Length == 0 && Paths.All(str => str.StartsWith(PathSegment)))
			    {
				    CommonPath = PathSegment;
			    }
			    else if (Paths.All(str => str.StartsWith(CommonPath + Separator + PathSegment)))
			    {
				    CommonPath += Separator + PathSegment;
			    }
			    else
			    {
				    break;
			    }
		    }

		    return CommonPath;
	    }
	}

	public class MenuItem
	{
		public MenuItem()
		{
			this.Items = new ObservableCollection<MenuItem>();
		}

		public string Title { get; set; }

		public ObservableCollection<MenuItem> Items { get; set; }
	}
}
