using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PropertyWriter.Annotation;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	[JsonObject(MemberSerialization.OptIn)]
	class Project
	{
		public ReactiveProperty<string> AssemblyPath { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<string> ProjectTypeName { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<string> SavePath { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<bool> IsValid { get; }

		public Project()
		{
			IsValid = AssemblyPath.Select(x => x != null)
				.CombineLatest(ProjectTypeName.Select(x => x != null), (x, y) => x && y)
				.CombineLatest(SavePath.Select(x => x != null), (x, y) => x && y)
				.ToReactiveProperty();
		}

		public Assembly GetAssembly() => Assembly.LoadFrom(AssemblyPath.Value);
		public Type GetProjectType(Assembly assembly) => assembly.GetTypes()
			.FirstOrDefault(x => x.Name == ProjectTypeName.Value);

		#region Serialize

		[JsonProperty]
		public string AssemblyPathValue
		{
			get { return AssemblyPath.Value; }
			set { AssemblyPath.Value = value; }
		}

		[JsonProperty]
		public string ProjectTypeNameValue
		{
			get { return ProjectTypeName.Value; }
			set { ProjectTypeName.Value = value; }
		}

		[JsonProperty]
		public string SavePathValue
		{
			get { return SavePath.Value; }
			set { SavePath.Value = value; }
		}

		#endregion
	}
}
