using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PropertyWriter.Annotation;
using PropertyWriter.Models.Exceptions;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Models.Serialize;
using Reactive.Bindings;
using JsonSerializer = PropertyWriter.Models.Serialize.JsonSerializer;
using System.Collections.ObjectModel;
using System.Threading;

namespace PropertyWriter.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Project
	{
		private ObservableCollection<Project> dependencies_ = new ObservableCollection<Project>();

		public ReactiveProperty<string> ProjectDir { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<string> AssemblyPath { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<string> ProjectTypeName { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<string> SavePath { get; } = new ReactiveProperty<string>();
		public ObservableCollection<ReactiveProperty<string>> DependenciesPathes { get; set; } = new ObservableCollection<ReactiveProperty<string>>();
		public ReactiveProperty<bool> IsValid { get; }

		public PropertyFactory Factory { get; }
		public ReactiveProperty<PropertyRoot> Root { get; } = new ReactiveProperty<PropertyRoot>();
		public ReadOnlyReactiveCollection<Project> Dependencies { get; }

		public Project()
		{
			IsValid = AssemblyPath.CombineLatest(ProjectTypeName, (x, y) =>
			{
				if (x == null || !File.Exists(GetProjectsContentPath(x)))
				{
					return false;
				}
				else
				{
					return GetProjectType(GetAssembly()) != null;
				}
			}).CombineLatest(SavePath.Select(x => x != null), (x, y) => x && y)
				.ToReactiveProperty();
			
			Dependencies = dependencies_.ToReadOnlyReactiveCollection();
			Factory = new PropertyFactory();
		}

		public Project(Project project)
			: this()
		{
			AssemblyPath.Value = project.AssemblyPath.Value;
			ProjectTypeName.Value = project.ProjectTypeName.Value;
			SavePath.Value = project.SavePath.Value;
			DependenciesPathes = project.DependenciesPathes;
			dependencies_ = project.dependencies_;
			Root.Value = project.Root.Value;
			ProjectDir.Value = project.ProjectDir.Value;
		}

		public Assembly GetAssembly()
		{
			if (AssemblyPath.Value == null)
			{
				return null;
			}

			string path = GetProjectsContentPath(AssemblyPath.Value);

			if(File.Exists(path))
			{
				return Assembly.LoadFrom(path);
			}
			return null;
		}

		public Type GetProjectType(Assembly assembly)
		{
			if (ProjectTypeName.Value == null)
			{
				return null;
			}
			
			return assembly.GetTypes()
				.FirstOrDefault(x => x.FullName == ProjectTypeName.Value);
		}

		public void InitializeRoot(Project[] dependencies)
		{
			Assembly assembly;
			try
			{
				assembly = GetAssembly();
			}
			catch (FileNotFoundException)
			{
				throw new PwProjectException("アセンブリが移動または削除されています。");
			}
			if (assembly == null)
			{
				throw new PwProjectException("アセンブリが移動または削除されています。");
			}

			var projectType = GetProjectType(assembly);
			if (projectType == null)
			{
				throw new PwProjectException("プロジェクト型定義が失われています。");
			}

			Root.Value = Factory.GetStructure(assembly, projectType, dependencies);
		}

		public static async Task<Project> LoadSettingAsync(string path)
		{
			using (var file = new StreamReader(path))
			{
				var project = JsonConvert.DeserializeObject<Project>(await file.ReadToEndAsync());
				project.ProjectDir.Value = Directory.GetParent(path).FullName;
				return project;
			}
		}

		public async Task LoadDataAsync()
		{
			await LoadDependencyAsync();
			InitializeRoot(dependencies_.ToArray());
			await LoadSerializedDataAsync();
		}

		public async Task LoadDependencyAsync()
		{
			var factories = new List<PropertyFactory>();
			foreach (var projectPath in DependenciesPathes)
			{
				dependencies_.Add(
					await LoadSettingAsync(
						GetProjectsContentPath(projectPath.Value)));
			}
			foreach (var subProject in dependencies_)
			{
				subProject.InitializeRoot(new Project[0]);
				await subProject.LoadSerializedDataAsync();
			}
		}

		private async Task LoadSerializedDataAsync()
		{
			var path = GetProjectsContentPath(SavePath.Value);
			if (!File.Exists(path))
			{
				return;
			}

			var deserializer = Root.Value.Type.GetMethods()
				.FirstOrDefault(x => x.GetCustomAttribute<PwDeserializerAttribute>() != null);
			if (deserializer != null)
			{
				await CustomSerializer.LoadDataAsync(deserializer, Root.Value, path);
			}
			else
			{
				await JsonSerializer.LoadDataAsync(Root.Value, path);
			}
		}

		public async Task SaveSettingAsync(string path)
		{
			using (var file = new StreamWriter(path))
			{
				var json = JsonConvert.SerializeObject(this, Formatting.Indented);
				await file.WriteLineAsync(json);
			}
		}

		public async Task SaveSerializedDataAsync()
		{
			var path = GetProjectsContentPath(SavePath.Value);
			var rootType = Root.Value.Type;
			var serializer = rootType.GetMethods()
				.FirstOrDefault(x => x.GetCustomAttribute<PwSerializerAttribute>() != null);
			if (serializer != null)
			{
				await CustomSerializer.SaveDataAsync(serializer, Root.Value, path);
			}
			else
			{
				await JsonSerializer.SaveDataAsync(Root.Value, path);
			}
		}

		public Project Clone()
		{
			var clone = new Project
			{
				AssemblyPathValue = this.AssemblyPathValue,
				ProjectTypeNameValue = this.ProjectTypeNameValue,
				SavePathValue = this.SavePathValue,
				DependenciesPathes = this.DependenciesPathes,
			};
			clone.ProjectDir.Value = this.ProjectDir.Value;
			return clone;
		}

		private string GetProjectsContentPath(string relativePath)
		{
			if (!Path.IsPathRooted(relativePath) && ProjectDir.Value != null)
			{
				return Path.Combine(ProjectDir.Value, relativePath);
			}
			return relativePath;
		}

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

		[JsonProperty]
		public string[] DependenciesPath
		{
			get { return DependenciesPathes.Select(x => x.Value).ToArray(); }
			set
			{
				DependenciesPathes.Clear();
				foreach (var item in value)
				{
					DependenciesPathes.Add(new ReactiveProperty<string>(item));
				}
			}
		}

		#endregion
	}
}
