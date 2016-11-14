using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PropertyWriter.Annotation;
using PropertyWriter.Model.Info;
using PropertyWriter.Model.Instance;
using PropertyWriter.ViewModel;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	class ReferencableMasterInfo
	{
		public Type Type { get; set; }
		public ReadOnlyReactiveCollection<object> Collection { get; set; }
	}

	class ModelFactory
	{
		private Dictionary<string, ReferencableMasterInfo> masters_;
		private Dictionary<Type, Type[]> subtypings_;

		public ModelFactory()
		{
			masters_ = new Dictionary<string, ReferencableMasterInfo>();
			subtypings_ = new Dictionary<Type, Type[]>();
		}

		public RootViewModel LoadStructure(Assembly assembly, Type projectType)
		{
			var types = assembly.GetTypes();
			var subtypings = types.Where(Helpers.IsAnnotatedType<PwSubtypingAttribute>).ToArray();
			var subtypes = types.Where(Helpers.IsAnnotatedType<PwSubtypeAttribute>).ToArray();
			subtypings_ = subtypings.ToDictionary(x => x, x => subtypes.Where(y => y.BaseType == x).ToArray());

			var masterMembers = projectType.GetMembers();
			var masters = LoadMastersInfo(masterMembers, true).ToArray();
			masters_ = masters.Where(x => x.Master is ComplicateCollectionModel)
				.ToDictionary(x => x.Key, x =>
				{
					var collection = x.Master as ComplicateCollectionModel;
					return new ReferencableMasterInfo()
					{
						Collection = collection?.Collection?.ToReadOnlyReactiveCollection(y => y.Value.Value),
						Type = x.Property.PropertyType.GetElementType(),
					};
				});

			var globals = LoadMastersInfo(masterMembers, false);
			var models = globals.Concat(masters).ToArray();

			return new RootViewModel(projectType, models.ToArray());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="masterMembers"></param>
		/// <param name="filterArrayType"></param>
		/// <returns></returns>
		/// <remarks>リストマスターが読み込まれていない場合<paramref name="filterArrayType"/>をfalseにすると例外を投げる</remarks>
		private IEnumerable<MasterInfo> LoadMastersInfo(MemberInfo[] masterMembers, bool filterArrayType)
		{
			foreach(var member in masterMembers)
			{
				var attr = member.GetCustomAttribute<PwMasterAttribute>();
				if (attr == null)
				{
					continue;
				}

				if (member.MemberType != MemberTypes.Property)
				{
					continue;
				}

				var property = (PropertyInfo) member;
				if(property.PropertyType.IsArray == filterArrayType)
				{
					yield return new MasterInfo(attr.Key, property, Create(property.PropertyType, attr.Name));
				}
			}
		}

		public IEnumerable<InstanceAndMemberInfo> LoadMembersInfo(Type type)
		{
			var properties = type.GetProperties()
				.Select(MakeModelAndInfo)
				.Where(x => x != null);

			return properties.Cast<InstanceAndMemberInfo>().ToArray();
		}

		private InstanceAndPropertyInfo MakeModelAndInfo(PropertyInfo info)
		{
			var memberAttr = info.GetCustomAttribute<PwMemberAttribute>();
			if(memberAttr != null)
			{
				return new InstanceAndPropertyInfo(
					info,
					Create(info.PropertyType, memberAttr.Name),
					memberAttr.Name);
			}

			var attr = info.GetCustomAttribute<PwReferenceMemberAttribute>();
			if(attr != null)
			{
				return new InstanceAndPropertyInfo(
					info,
					CreateReference(info.PropertyType, attr.MasterKey, attr.IdFieldName, attr.Name),
					attr.Name);
			}

			return null;
		}


		public IPropertyModel CreateReference(Type type, string masterKey, string idMemberName, string title)
		{
			var propertyType = TypeRecognizer.ParseType(type);
			switch(propertyType)
			{
			case PropertyKind.Integer:
				return new ReferenceByIntModel(masters_[masterKey], idMemberName)
				{
					Title = { Value = title }
				};
			default:
				throw new InvalidOperationException("ID参照はInt32のみがサポートされます。");
			}
		}

		public IPropertyModel Create(Type type, string title)
		{
			var propertyType = TypeRecognizer.ParseType(type);
			var model = Create(propertyType, type);
			model.Title.Value = title;
			return model;
		}

		private IPropertyModel Create(PropertyKind propertyType, Type type)
		{
			switch(propertyType)
			{
			case PropertyKind.Integer: return new IntModel();
			case PropertyKind.Boolean: return new BoolModel();
			case PropertyKind.String: return new StringModel();
			case PropertyKind.Float: return new FloatModel();
			case PropertyKind.Enum: return new EnumModel(type);
			case PropertyKind.Class: return new ClassModel(type, this);
			case PropertyKind.Struct: return new StructModel(type, this);
			case PropertyKind.BasicCollection: return new BasicCollectionModel(type, this);
			case PropertyKind.ComplicateCollection: return new ComplicateCollectionModel(type, this);
			case PropertyKind.SubtypingClass: return new SubtypingModel(type, this, subtypings_[type]);

			case PropertyKind.Unknown: return null;
			default: return null;
			}
		}
	}
}
