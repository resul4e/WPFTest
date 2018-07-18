using System;
using System.Collections.Generic;
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

namespace TestApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : NavigationWindow
	{
		public MainWindow()
		{

			using (SvnClient client = new SvnClient())
			{

				SvnUriTarget target = new SvnUriTarget(System.IO.Path.GetFullPath("D:/school/programmen/WPFTest/SVN Server"));

				if (client.CheckOut(target, "CodeTest", ))
				{
					MessageBox.Show("success");
				}
				else
				{
					MessageBox.Show("Fail");
				}
			}

				InitializeComponent();
		}
	}
}
