using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Helpers
{
	public class FileHelper
	{
		public static string FindCommonPath(string _separator, List<string> _paths)
		{
			string commonPath = String.Empty;
			List<string> separatedPath = _paths
				.First(str => str.Length == _paths.Max(st2 => st2.Length))
				.Split(new string[] { _separator }, StringSplitOptions.RemoveEmptyEntries)
				.ToList();

			foreach (string pathSegment in separatedPath.AsEnumerable())
			{
				if (commonPath.Length == 0 && _paths.All(str => str.StartsWith(pathSegment)))
				{
					commonPath = pathSegment;
				}
				else if (_paths.All(str => str.StartsWith(commonPath + _separator + pathSegment)))
				{
					commonPath += _separator + pathSegment;
				}
				else
				{
					break;
				}
			}

			return commonPath;
		}
	}
}
