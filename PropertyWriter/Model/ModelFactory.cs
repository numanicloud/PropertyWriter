using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PropertyWriter.Annotation;
using PropertyWriter.Model.Info;
using PropertyWriter.Model.Instance;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	class ModelFactory
	{
		private Dictionary<Type, ReadOnlyReactiveCollection<object>> masters_;
		private Dictionary<Type, Type[]> subtypings_;

		public ModelFactory()
		{
			masters_ = new Dictionary<Type, ReadOnlyReactiveCollection<object>>();
			subtypings_ = new Dictionary<Type, Type[]>();
		}

		public MasterInfo[] LoadStructure(Assembly assembly, Type projectType)
		{
			var types = assembly.GetTypes();
			var subtypings = types.Where(Helpers.IsAnnotatedType<PwSubtypingAttribute>).ToArray();
			var subtypes = types.Where(Helpers.IsAnnotatedType<PwSubtypeAttribute>).ToArray();
			subtypings_ = subtypings.ToDictionary(x => x, x => subtypes.Where(y => y.BaseType == x).ToArray());

			var masterMembers = projectType.GetMembers();
			var masters = LoadMastersInfo(masterMembers).ToArray();
			masters_ = masters.Where(x => x.Master is ComplicateCollectionModel)
				.ToDictionary(x => x.Type, x =>
				{
					var collection = x.Master as ComplicateCollectionModel;
					return collection?.Collection?.ToReadOnlyReactiveCollection(y => y.Value.Value);
				});

			return masters;
		}


		private IEnumerable<MasterInfo> LoadMastersInfo(MemberInfo[] masterMembers)
		{
			foreach (var member in masterMembers)
			{
				var attr = member.GetCustomAttribute<PwMasterAttribute>();
				if (attr != null)
				{
					if (member.DeclaringType?.IsArray == true)
					{
						yield return MasterInfo.ForMaster(member.DeclaringType, this, attr.Name);
					}
					else
					{
						yield return MasterInfo.ForGlobal(member.DeclaringType, this, attr.Name);
					}
				}
			}
		}

		public IEnumerable<InstanceAndMemberInfo> LoadMembersInfo(Type type)
		{
			var properties = type.GetProperties()
				.Select(MakeModelAndInfo)
				.Where(x => x != null);
			var fields = type.GetFields()
				.Select(MakeModelAndInfo)
				.Where(x => x != null);

			return properties.Cast<InstanceAndMemberInfo>().Concat(fields).ToArray();
		}

		private InstanceAndPropertyInfo MakeModelAndInfo(PropertyInfo info)
		{
			var memberAttr = info.GetCustomAttribute<PwMemberAttribute>();
			if (memberAttr != null)
			{
				return new InstanceAndPropertyInfo(
					info,
					Create(info.PropertyType, memberAttr.Name),
					memberAttr.Name);
			}

			var attr = info.GetCustomAttribute<PwReferenceMemberAttribute>();
			if (attr != null)
			{
				return new InstanceAndPropertyInfo(
					info,
					CreateReference(info.PropertyType, attr.TargetType, attr.IdFieldName, attr.Name),
					attr.Name);
			}

			return null;
		}

		private InstanceAndFieldInfo MakeModelAndInfo(FieldInfo info)
		{
			var memberAttr = info.GetCustomAttribute<PwMemberAttribute>();
			if (memberAttr != null)
			{
				return new InstanceAndFieldInfo(
					info,
					Create(info.FieldType, memberAttr.Name),
					memberAttr.Name);
			}

			var attr = info.GetCustomAttribute<PwReferenceMemberAttribute>();
			if (attr != null)
			{
				return new InstanceAndFieldInfo(
					info,
					CreateReference(info.FieldType, attr.TargetType, attr.IdFieldName, attr.Name),
					attr.Name);
			}

			return null;
		}


		public IPropertyModel CreateReference(Type type, Type targetType, string idMemberName, string title)
		{
			var propertyType = TypeRecognizer.ParseType(type);
			switch (propertyType)
			{
				case PropertyKind.Integer:
					return new ReferenceByIntModel(targetType, idMemberName)
					{
						Source = masters_[targetType],
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
			switch (propertyType)
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
