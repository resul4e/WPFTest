﻿using System;
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
				.First(_str => _str.Length == _paths.Max(_st2 => _st2.Length))
				.Split(new string[] { _separator }, StringSplitOptions.RemoveEmptyEntries)
				.ToList();

			foreach (string pathSegment in separatedPath.AsEnumerable())
			{
				if (commonPath.Length == 0 && _paths.All(_str => _str.StartsWith(pathSegment)))
				{
					commonPath = pathSegment;
				}
				else if (_paths.All(_str => _str.StartsWith(commonPath + _separator + pathSegment)))
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
