using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PropertyWriter.Model
{
	class InstanceFactory
	{
		public static IInstance Create( Type type )
		{
			var propertyType = TypeParser.ParseType( type );
			return Create( propertyType, type );
		}

		private static IInstance Create( PropertyKind propertyType, Type type )
		{
			switch( propertyType )
			{
			case PropertyKind.Integer: return new IntInstance();
			case PropertyKind.Boolean: return new BoolInstance();
			case PropertyKind.String: return new StringInstance();
			case PropertyKind.Float: return new FloatInstance();
			case PropertyKind.Enum: return new EnumInstance( type );
			case PropertyKind.Class: return new ClassInstance( type );
			case PropertyKind.Struct: return new StructInstance( type );
			case PropertyKind.BasicCollection: return new BasicCollectionInstance( type );
			case PropertyKind.ComplicateCollection: return new ComplicateCollectionInstance( type );

			case PropertyKind.Unknown: return null;
			default: return null;
			}
		}
	}
}
