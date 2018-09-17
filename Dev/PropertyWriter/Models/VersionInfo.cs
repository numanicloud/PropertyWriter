using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Models
{
	class VersionInfo
	{
		private static readonly Version AppVersion = new Version(1, 0, 0, 0);

		public static string GetAppVersionString()
		{
			return $"{AppVersion.Major}.{AppVersion.Minor}.{AppVersion.Build}";
		}
	}
}
