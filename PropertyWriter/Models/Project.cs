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

namespace PropertyWriter.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    class Project
    {
        private ObservableCollection<ReactiveProperty<string>> dependenciesPathes_ = new ObservableCollection<ReactiveProperty<string>>();
        private ObservableCollection<Project> dependencies_ = new ObservableCollection<Project>();

        public ReactiveProperty<string> AssemblyPath { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ProjectTypeName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> SavePath { get; } = new ReactiveProperty<string>();
        public ObservableCollection<ReactiveProperty<string>> DependenciesPathes { get; }
        public ReactiveProperty<bool> IsValid { get; }

        public ReactiveProperty<PropertyRoot> Root { get; } = new ReactiveProperty<PropertyRoot>();
        public ReadOnlyReactiveCollection<Project> Dependencies { get; }

        public Project()
        {
            IsValid = AssemblyPath.CombineLatest(ProjectTypeName, (x, y) =>
            {
                if (x == null || !File.Exists(x))
                {
                    return false;
                }
                else
                {
                    return GetProjectType(GetAssembly()) != null;
                }
            }).CombineLatest(SavePath.Select(x => x != null), (x, y) => x && y)
                .ToReactiveProperty();

            DependenciesPathes = dependenciesPathes_;
            Dependencies = dependencies_.ToReadOnlyReactiveCollection();
        }

        public Project(Project project)
            : this()
        {
            AssemblyPath.Value = project.AssemblyPath.Value;
            ProjectTypeName.Value = project.ProjectTypeName.Value;
            SavePath.Value = project.SavePath.Value;
			dependenciesPathes_ = project.DependenciesPathes;
			dependencies_ = project.dependencies_;
            Root.Value = project.Root.Value;
        }

        public Assembly GetAssembly()
        {
            if (AssemblyPath.Value == null)
            {
                return null;
            }
            return Assembly.LoadFrom(AssemblyPath.Value);
        }

        public Type GetProjectType(Assembly assembly)
        {
            if (ProjectTypeName.Value == null)
            {
                return null;
            }
            return assembly.GetTypes()
                .FirstOrDefault(x => x.Name == ProjectTypeName.Value);
        }

        public void InitializeRoot(PropertyFactory loader, PropertyFactory[] dependencies)
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

            var projectType = GetProjectType(assembly);
            if (projectType == null)
            {
                throw new PwProjectException("プロジェクト型定義が失われています。");
            }
			
			Root.Value = loader.GetStructure(assembly, projectType, dependencies);
        }

        public static async Task<Project> LoadSettingAsync(string path)
        {
            using (var file = new StreamReader(path))
            {
                return JsonConvert.DeserializeObject<Project>(await file.ReadToEndAsync());
            }
        }

        public async Task LoadDataAsync(bool isRootProject)
		{
			var factories = new List<PropertyFactory>();
			if (isRootProject)
			{
				foreach (var projectPath in dependenciesPathes_)
				{
					dependencies_.Add(await LoadSettingAsync(projectPath.Value));
				}
				foreach (var subProject in dependencies_)
				{
					var f = new PropertyFactory();
					subProject.InitializeRoot(f, new PropertyFactory[0]);
					await subProject.LoadSerializedDataAsync();
					factories.Add(f);
				}
			}

			InitializeRoot(new PropertyFactory(), factories.ToArray());
			await LoadSerializedDataAsync();
		}

		private async Task LoadSerializedDataAsync()
		{
			var deserializer = Root.Value.Type.GetMethods()
				.FirstOrDefault(x => x.GetCustomAttribute<PwDeserializerAttribute>() != null);
			try
			{
				if (deserializer != null)
				{
					await CustomSerializer.LoadDataAsync(deserializer, Root.Value, SavePath.Value);
				}
				else
				{
					await JsonSerializer.LoadDataAsync(Root.Value, SavePath.Value);
				}
			}
			catch (FileNotFoundException)
			{
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

        public async Task SaveDataAsync()
        {
            var rootType = Root.Value.Type;
            var serializer = rootType.GetMethods()
                .FirstOrDefault(x => x.GetCustomAttribute<PwSerializerAttribute>() != null);
            if (serializer != null)
            {
                await CustomSerializer.SaveDataAsync(serializer, Root.Value, SavePath.Value);
            }
            else
            {
                await JsonSerializer.SaveDataAsync(Root.Value, SavePath.Value);
            }
        }

        public Project Clone()
        {
            return new Project
            {
                AssemblyPathValue = this.AssemblyPathValue,
                ProjectTypeNameValue = this.ProjectTypeNameValue,
                SavePathValue = this.SavePathValue,
                dependenciesPathes_ = this.dependenciesPathes_,
            };
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
            get { return dependenciesPathes_.Select(x => x.Value).ToArray(); }
            set
            {
                dependenciesPathes_.Clear();
                foreach (var item in value)
                {
                    dependenciesPathes_.Add(new ReactiveProperty<string>(item));
                }
            }
        }

        #endregion
    }
}
