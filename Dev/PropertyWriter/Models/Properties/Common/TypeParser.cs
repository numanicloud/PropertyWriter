using System;
using PropertyWriter.Annotation;
using System.Linq;

namespace PropertyWriter.Models.Properties.Common
{
	enum PropertyKind
	{
		Integer, Boolean, String, Unknown,
		Float, Enum,
		Class, Struct, SubtypingClass,
		BasicCollection,
		ComplicateCollection,
	}

	class TypeRecognizer
	{
		public static PropertyKind ParseType(Type type)
		{
			switch(type.Name)
			{
			case "Int32": return PropertyKind.Integer;
			case "Boolean": return PropertyKind.Boolean;
			case "String": return PropertyKind.String;
			case "Single": return PropertyKind.Float;
			case "IEnumerable`1":
				return IsComplecateCollection(type.GenericTypeArguments[0])
					? PropertyKind.ComplicateCollection
					: PropertyKind.BasicCollection;
			}
			
			if (type.IsArray)
			{
				return IsComplecateCollection(type.GetElementType())
					? PropertyKind.ComplicateCollection
					: PropertyKind.BasicCollection;
			}
			if(type.IsEnum)
			{
				return PropertyKind.Enum;
			}
			else if(type.IsClass || type.IsInterface)
			{
			    if (Helpers.IsAnnotatedType<PwSubtypingAttribute>(type))
			    {
			        return PropertyKind.SubtypingClass;
			    }
				return PropertyKind.Class;
			}
			else if(type.IsValueType)
			{
				return PropertyKind.Struct;
			}

			return PropertyKind.Unknown;
		}

		private static bool IsComplecateCollection(Type elementType)
		{
			var element = ParseType(elementType);
			return element == PropertyKind.Class || element == PropertyKind.Struct || element == PropertyKind.SubtypingClass;
		}
	}
}
