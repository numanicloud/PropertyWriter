using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Launcher
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("PropertyWriter を起動中...");
			Directory.SetCurrentDirectory("Core\\");
			Process.Start("PropertyWriter.exe");
		}
	}
}
