using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PropertyWriter.Model.Instance;

namespace PropertyWriter.Model
{
	class InstanceFactory
	{
		public static IPropertyModel CreateReference(Type type, Type targetType, string idMemberName)
		{
			var propertyType = TypeParser.ParseType(type);
			switch (propertyType)
			{
			case PropertyKind.Integer:
				return new ReferenceByIntModel(targetType, idMemberName);
			default:
				throw new InvalidOperationException("ID参照はInt32のみがサポートされます。");
			}
		}

		public static IPropertyModel Create( Type type )
		{
			var propertyType = TypeParser.ParseType( type );
			return Create( propertyType, type );
		}

		private static IPropertyModel Create( PropertyKind propertyType, Type type )
		{
			switch( propertyType )
			{
			case PropertyKind.Integer: return new IntModel();
			case PropertyKind.Boolean: return new BoolModel();
			case PropertyKind.String: return new StringModel();
			case PropertyKind.Float: return new FloatModel();
			case PropertyKind.Enum: return new EnumModel( type );
			case PropertyKind.Class: return new ClassModel( type );
			case PropertyKind.Struct: return new StructModel( type );
			case PropertyKind.BasicCollection: return new BasicCollectionModel( type );
			case PropertyKind.ComplicateCollection: return new ComplicateCollectionModel( type );

			case PropertyKind.Unknown: return null;
			default: return null;
			}
		}
	}
}
