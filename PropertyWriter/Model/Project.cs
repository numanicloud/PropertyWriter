using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	class Project
	{
		public ReactiveProperty<string> AssemblyPath { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<string> ProjectTypeName { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<string> SavePath { get; } = new ReactiveProperty<string>();

		public Assembly GetAssembly() => Assembly.LoadFrom(AssemblyPath.Value);
		public Type GetProjectType(Assembly assembly) => assembly.GetTypes()
			.FirstOrDefault(x => x.Name == ProjectTypeName.Value);
	}
}
