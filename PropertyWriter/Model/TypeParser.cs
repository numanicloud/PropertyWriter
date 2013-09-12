using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertyWriter.Model
{
	enum PropertyKind
	{
		Integer, Boolean, String, Unknown,
		Float,
		Enum,
		Class,
		Struct,
		BasicCollection,
		ComplicateCollection
	}

	class TypeParser
	{
		public static PropertyKind ParseType( Type type )
		{
			switch( type.Name )
			{
			case "Int32": return PropertyKind.Integer;
			case "Boolean": return PropertyKind.Boolean;
			case "String": return PropertyKind.String;
			case "Single": return PropertyKind.Float;
			case "IEnumerable`1":
				return IsComplecateCollection( type )
					? PropertyKind.ComplicateCollection
					: PropertyKind.BasicCollection;
			}

			if( type.IsEnum )
			{
				return PropertyKind.Enum;
			}
			else if( type.IsClass || type.IsInterface )
			{
				return PropertyKind.Class;
			}
			else if( type.IsValueType )
			{
				return PropertyKind.Struct;
			}

			return PropertyKind.Unknown;
		}

		private static bool IsComplecateCollection( Type type )
		{
			var element = ParseType( type.GenericTypeArguments[0] );
			return element == PropertyKind.Class || element == PropertyKind.Struct;
		}
	}
}
