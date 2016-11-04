using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using PropertyWriter.Model.Instance;
using PropertyWriter.ViewModel.Instance;

namespace PropertyWriter.Model
{
	class JsonSerializer
	{
		public static async Task SaveData(MasterInfo[] roots, string savePath)
		{
			foreach(var masterInfo in roots)
			{
				var path = Path.Combine(savePath, masterInfo.Type.Name + ".json");
				using(var file = new StreamWriter(path))
				{
					var json = JsonConvert.SerializeObject(masterInfo.Master.Value.Value, Formatting.Indented);
					await file.WriteLineAsync(json);
				}
			}
		}

		public static async Task<bool> LoadData(MasterInfo[] roots, string savePath)
		{
			bool result = true;
			var references = new List<Tuple<ReferenceByIntModel, int>>();
			foreach(var masterInfo in roots)
			{
				var path = Path.Combine(savePath, masterInfo.Type.Name + ".json");
				using(var file = new StreamReader(path))
				{
					var obj = JToken.Parse(await file.ReadToEndAsync());
					result &= WriteToModel(masterInfo.Master, obj, references);
				}
				foreach (var reference in references)
				{
					reference.Item1.SetItemById(reference.Item2);
				}
			}
			return result;
		}


		private static bool WriteValueIfMatches<TModel>(IPropertyModel model, JToken token, Action<TModel, object> assign)
			where TModel : class, IPropertyModel
		{
			var valModel = model as TModel;
			if(valModel != null)
			{
				var val = (JValue)token;
				assign(valModel, val.Value);
				return true;
			}
			return false;
		}

		private static bool WriteArrayIfMatches<TModel>(IPropertyModel model, JToken token, List<Tuple<ReferenceByIntModel, int>> references)
			where TModel : class, IPropertyModel, ICollectionModel
		{
			var collectionModel = model as TModel;
			if(collectionModel != null)
			{
				var array = (JArray)token;
				bool result = true;
				foreach(var jtoken in array)
				{
					var element = collectionModel.AddNewElement();
					result &= WriteToModel(element, jtoken, references);
				}
				return result;
			}
			return false;
		}

		private static bool WriteObjectIfMatches<TModel>(IPropertyModel model, JToken token, List<Tuple<ReferenceByIntModel, int>> references)
			where TModel : class, IPropertyModel, IStructureModel
		{
			var classModel = model as TModel;
			if(classModel != null)
			{
				var members = classModel.Members;
				bool result = true;
				foreach(var property in members)
				{
					var obj = (JObject)token;
					var val = obj.GetValue(property.MemberName);
					if (val == null)
					{
						return false;
					}
					result &= WriteToModel(property.Model, val, references);
				}
				return result;
			}
			return false;
		}

		private static bool WriteToModel(IPropertyModel model, JToken token, List<Tuple<ReferenceByIntModel, int>> references)
		{
			if(token == null)
			{
				return true;
			}

			try
			{
				if(WriteValueIfMatches<IntModel>(model, token, (m, x) => m.IntValue.Value = (int)(long)x)) return true;
				if(WriteValueIfMatches<StringModel>(model, token, (m, x) => m.StringValue.Value = (string)x)) return true;
				if(WriteValueIfMatches<BoolModel>(model, token, (m, x) => m.BoolValue.Value = (bool)x)) return true;
				if(WriteValueIfMatches<FloatModel>(model, token, (m, x) => m.FloatValue.Value = (float)(double)x)) return true;
				if (WriteValueIfMatches<EnumModel>(model, token, ConvertEnum)) return true;

				if(WriteObjectIfMatches<ClassModel>(model, token, references)) return true;
				if(WriteObjectIfMatches<StructModel>(model, token, references)) return true;

				var refByIntModel = model as ReferenceByIntModel;
				if(refByIntModel != null)
				{
					var val = (JValue)token;
					references.Add(new Tuple<ReferenceByIntModel, int>(refByIntModel, (int)(long)val.Value));
					return true;
				}

				var subtypingModel = model as SubtypingModel;
				if(subtypingModel != null)
				{
					foreach(var type in subtypingModel.AvailableTypes)
					{
						subtypingModel.SelectedType.Value = type;
						bool success = WriteToModel(subtypingModel.Model.Value, token, references);
						if(success)
						{
							return true;
						}
					}
					return false;
				}

				if(WriteArrayIfMatches<ComplicateCollectionModel>(model, token, references)) return true;
				if(WriteArrayIfMatches<BasicCollectionModel>(model, token, references)) return true;

				return false;
			}
			catch(Exception e)
			{
				Debugger.Log(0, "Info", e.Message + "\n");
				Debugger.Log(0, "Info", e.StackTrace + "\n");
				Debugger.Log(0, "Info", $"{model} <- {token}\n");
				return false;
			}
		}

		private static void ConvertEnum(EnumModel model, object obj)
		{
			var val = model.EnumValues.FirstOrDefault(x => x.ToString() == model.Type.GetEnumName(obj));
			model.EnumValue.Value = val;
		}
	}
}
