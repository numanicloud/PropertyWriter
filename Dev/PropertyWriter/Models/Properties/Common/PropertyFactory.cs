using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PropertyWriter.Annotation;
using PropertyWriter.Models.Info;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels;
using PropertyWriter.ViewModels.Properties;
using Reactive.Bindings;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PropertyWriter.Models.Properties.Common
{
	public class ReferencableMasterInfo
	{
		public Type Type { get; set; }
		public ReadOnlyReactiveCollection<object> Collection { get; set; }
	}

	public class PropertyFactory
	{
		private MasterLoader loader;

		public ReadOnlyDictionary<string, ReferencableMasterInfo> Masters => loader.Masters;
		

		public PropertyFactory()
		{
			loader = new MasterLoader(this);
		}

		public PropertyRoot GetStructure(Assembly assembly, Type projectType, Project[] dependencies)
		{
			return loader.LoadStructure(assembly, projectType, dependencies);
		}

		public IEnumerable<IPropertyModel> CreateForMembers(Type type)
		{
			var properties = type.GetProperties();

			if (properties.Any(x => x.PropertyType == type))
			{
				throw new InvalidOperationException($"クラス {type.Name} は依存関係が循環しています。");
			}

			var models = properties.Select(CreateFromProperty)
				.Where(x => x != null)
				.ToArray();

			LoadBackwardBind(type, models);

			return models.Cast<IPropertyModel>().ToArray();
		}

		public IPropertyModel Create(Type type, string title)
		{
			var propertyType = TypeRecognizer.ParseType(type);
			if (!type.IsPublic && !type.IsNestedPublic)
			{
				throw new ArgumentException($"型 {type.FullName} がパブリックではありませんでした。", nameof(type));
			}
			var model = Create(propertyType, type);
			model.Title.Value = title;
			return model;
		}
		

		private static void LoadBackwardBind(Type type, IPropertyModel[] properties)
		{
			var references = new Dictionary<string, ReferenceByIntProperty>();
			foreach (var prop in properties)
			{
				if (prop is ReferenceByIntProperty refByInt)
				{
					references[prop.PropertyInfo.Name] = refByInt;
				}
			}

			foreach (var prop in type.GetProperties())
			{
				var bindAttr = prop.GetCustomAttribute<PwBindBackAttribute>();
				if (bindAttr != null)
				{
					references[bindAttr.PropertyName].PropertyToBindBack = prop;
				}
			}
		}

		private IPropertyModel CreateFromProperty(PropertyInfo info)
		{
			IPropertyModel result = null;
			void SetInfo()
			{
				if (result != null)
				{
					result.PropertyInfo = info;
				}
			}

			var memberAttr = info.GetCustomAttribute<PwMemberAttribute>();
			if (memberAttr != null)
			{
				result = Create(info.PropertyType, memberAttr.Name ?? info.Name);
				SetInfo();
				return result;
			}

			var multiLineAttr = info.GetCustomAttribute<PwMultiLineTextMemberAttribute>();
			if (multiLineAttr != null)
			{
				result = CreateMultiLine(info.PropertyType, multiLineAttr.Name ?? info.Name);
				SetInfo();
				return result;
			}

			var attr = info.GetCustomAttribute<PwReferenceMemberAttribute>();
			if (attr != null)
			{
				result = CreateReference(info.PropertyType, attr.MasterKey, attr.IdFieldName, attr.Name ?? info.Name);
				SetInfo();
				return result;
			}

			return null;
		}

		private IPropertyModel CreateMultiLine(Type type, string title)
		{
			var propertyType = TypeRecognizer.ParseType(type);
			switch (propertyType)
			{
			case PropertyKind.String:
				return new StringProperty(true)
				{
					Title = { Value = title }
				};
			default: throw new Exception();
			}
		}

		private IPropertyModel CreateReference(Type type, string masterKey, string idMemberName, string title)
		{
			var propertyType = TypeRecognizer.ParseType(type);
			switch (propertyType)
			{
			case PropertyKind.Integer:
				if (!loader.Masters.ContainsKey(masterKey))
				{
					throw new KeyNotFoundException($"[PwReferenceMember] 属性で指定されたマスターキー \"{masterKey}\" を持つ [PwMaster] 属性のついたメンバーがプロジェクトに含まれていません。");
				}
				return new ReferenceByIntProperty(loader.Masters[masterKey], idMemberName)
				{
					Title = { Value = title }
				};

			case PropertyKind.BasicCollection:
				if (!loader.Masters.ContainsKey(masterKey))
				{
					throw new KeyNotFoundException($"[PwReferenceMember] 属性で指定されたマスターキー \"{masterKey}\" を持つ [PwMaster] 属性のついたメンバーがプロジェクトに含まれていません。");
				}
				return new ReferenceByIntCollectionProperty(loader.Masters[masterKey], idMemberName, this)
				{
					Title = { Value = title }
				};

			default:
				throw new InvalidOperationException("ID参照をするプロパティの型は int, int[] 型のみがサポートされます。");
			}
		}

		private IPropertyModel Create(PropertyKind propertyType, Type type)
		{
			switch (propertyType)
			{
			case PropertyKind.Integer: return new IntProperty();
			case PropertyKind.Boolean: return new BoolProperty();
			case PropertyKind.String: return new StringProperty(false);
			case PropertyKind.Float: return new FloatProperty();
			case PropertyKind.Enum: return new EnumProperty(type);
			case PropertyKind.Class: return new ClassProperty(type, this);
			case PropertyKind.Struct: return new StructProperty(type, this);
			case PropertyKind.BasicCollection: return new BasicCollectionProperty(type, this);
			case PropertyKind.ComplicateCollection: return new ComplicateCollectionProperty(type, this);
			case PropertyKind.SubtypingClass: return new SubtypingProperty(type, this, loader.Subtypes[type]);

			case PropertyKind.Unknown: return null;
			default: return null;
			}
		}
	}
}
