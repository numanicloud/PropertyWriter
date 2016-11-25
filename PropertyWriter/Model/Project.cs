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
using PropertyWriter.ViewModel;
using System.IO;

namespace PropertyWriter.Model
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
            if(deserializer != null)
            {
                object value;
                using (var file = File.OpenRead(project.SavePath.Value))
                {
                    try
                    {
                        value = deserializer.Invoke(null, new object[] { file });
                    }
                    catch (ArgumentException)
                    {
                        throw new PwSerializeMethodException("デシリアライズ メソッドのシグネチャが不正です。");
                    }
                    catch(TargetParameterCountException)
                    {
                        throw new PwSerializeMethodException("デシリアライズ メソッドのシグネチャが不正です。");
                    }

                    if (value is Task t)
                    {
                        await t;
                        var resultProperty = typeof(Task<>).MakeGenericType(rootType).GetProperty(nameof(Task<object>.Result));
                        value = resultProperty.GetValue(t);
                    }
                    if(value == null)
                    {
                        throw new Exception("データ ファイルが壊れています。");
                    }
                }
                await ModelConverter.LoadValueToRootAsync(project.Root.Value, value);
            }
            else
            {
                await JsonSerializer.LoadDataAsync(project.Root.Value, project.SavePath.Value);
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
                var backup = MakeBackUp();
                try
                {
                    using (var file = File.OpenWrite(SavePath.Value))
                    {
                        var value = Root.Value.Structure.Value.Value;
                        try
                        {
                            var ret = serializer.Invoke(null, new object[] { value, file });
                            if (ret != null && ret is Task t)
                            {
                                await t;
                            }
                        }
                        catch (ArgumentException)
                        {
                            throw new PwSerializeMethodException("シリアライズ用メソッドのシグネチャが不正です。");
                        }
                    }
                }
                catch (Exception)
                {
                    WriteBackUp(backup);
                    throw;
                }
            }
            else
            {
                await JsonSerializer.SaveDataAsync(Root.Value, SavePath.Value);
            }
        }

        private void WriteBackUp(byte[] backup)
        {
            using (var file = new BinaryWriter(File.OpenWrite(SavePath.Value)))
            {
                file.Write(backup);
            }
        }

        private byte[] MakeBackUp()
        {
            List<byte> bytes = new List<byte>();
            using (var file = new BinaryReader(File.OpenRead(SavePath.Value)))
            {
                int length = 0;
                do
                {
                    var buffer = new byte[1024];
                    length = file.Read(buffer, 0, 1024);
                    bytes.AddRange(buffer.Take(length));
                } while (length == 1024);
            }
            return bytes.ToArray();
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
