using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PropertyWriter.Annotation;
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

		public MasterInfo[] LoadData(string assemblyPath)
		{
			var types = Assembly.LoadFrom(assemblyPath).GetTypes();
			
			var subtypings = types.Where(Helpers.IsAnnotatedType<PwSubtypingAttribute>).ToArray();
			var subtypes = types.Where(Helpers.IsAnnotatedType<PwSubtypeAttribute>).ToArray();
			subtypings_ = subtypings.ToDictionary(x => x, x => subtypes.Where(y => y.BaseType == x).ToArray());

			var masters = EntityLoader.LoadMasters(types, this);
			masters_ = masters.Where(x => x.Master is ComplicateCollectionModel)
				.ToDictionary(x => x.Type, x =>
			{
				var collection = x.Master as ComplicateCollectionModel;
				return collection?.Collection?.ToReadOnlyReactiveCollection(y => y.Value.Value);
			});

			var globals = EntityLoader.LoadGlobals(types, this);

			return globals.Concat(masters).ToArray();
		}

		public IPropertyModel CreateReference(Type type, Type targetType, string idMemberName)
		{
			var propertyType = TypeParser.ParseType(type);
			switch (propertyType)
			{
				case PropertyKind.Integer:
					return new ReferenceByIntModel(targetType, idMemberName)
					{
						Source = masters_[targetType]
					};
				default:
					throw new InvalidOperationException("ID参照はInt32のみがサポートされます。");
			}
		}

		public IPropertyModel Create(Type type)
		{
			var propertyType = TypeParser.ParseType(type);
			return Create(propertyType, type);
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
				case PropertyKind.SubtypingClass:
					return new SubtypingModel(type, this)
					{
						AvailableTypes = subtypings_[type]
					};
				case PropertyKind.Array:
				{
					var element = TypeParser.ParseType(type.GetElementType());
					var ienumerable = typeof(IEnumerable<>).MakeGenericType(type.GetElementType());
					return element == PropertyKind.Class || element == PropertyKind.Struct
						? new ComplicateCollectionModel(ienumerable, this)
						: (IPropertyModel)new BasicCollectionModel(ienumerable, this);
				}

				case PropertyKind.Unknown: return null;
				default: return null;
			}
		}
	}
}
