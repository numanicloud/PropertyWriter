using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	class Project
	{
		public ReactiveProperty<string> AssemblyPath { get; set; }
		public ReactiveProperty<string> SavePath { get; set; }
	}
}
