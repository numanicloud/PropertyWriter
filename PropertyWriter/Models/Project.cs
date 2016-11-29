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

namespace PropertyWriter.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	class Project
	{
		public ReactiveProperty<string> AssemblyPath { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<string> ProjectTypeName { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<string> SavePath { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<bool> IsValid { get; }
        public ReactiveProperty<PropertyRoot> Root { get; } = new ReactiveProperty<PropertyRoot>();

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

        public void Initialize()
        {
            var modelFactory = new PropertyFactory();
            var assembly = GetAssembly();
            Root.Value = modelFactory.GetStructure(assembly, GetProjectType(assembly));
        }

        public static async Task<Project> LoadAsync(string path)
        {
            Project project;
            using (var file = new StreamReader(path))
            {
                project = JsonConvert.DeserializeObject<Project>(await file.ReadToEndAsync());
            }

            project.Initialize();

            var rootType = project.Root.Value.Type;
            var deserializer = rootType.GetMethods()
                .FirstOrDefault(x => x.GetCustomAttribute<PwDeserializerAttribute>() != null);
			try
			{
				if (deserializer != null)
				{
					await CustomSerializer.LoadDataAsync(deserializer, project.Root.Value, project.SavePath.Value);
				}
				else
				{
					await JsonSerializer.LoadDataAsync(project.Root.Value, project.SavePath.Value);
				}
			}
			catch (FileNotFoundException)
			{
			}

            return project;
        }

        public async Task SaveAsync(string path)
        {
            using (var file = new StreamWriter(path))
            {
                var json = JsonConvert.SerializeObject(this, Formatting.Indented);
                await file.WriteLineAsync(json);
            }

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
