using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertyWriter.Model
{
	class InstanceConverter
	{
		public static object Convert(object obj, Type type)
		{
			if(obj is IEnumerable<object> && IsIEnumerable(type))
			{
				return Convert((IEnumerable<object>)obj, type.GenericTypeArguments[0]);
			}
			return obj;
		}

		private static bool IsIEnumerable(Type type)
		{
			return EqualIEnumerable(type)
				|| type.GetInterfaces().Any(EqualIEnumerable);
		}

		private static bool EqualIEnumerable(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
		}

		private static object Convert(IEnumerable<object> objectArray, Type elementType)
		{
			dynamic ofType = objectArray.Where(_ => _.GetType() == elementType).ToArray();
			dynamic array = Array.CreateInstance(elementType, ofType.Length);

			for(int i = 0; i < ofType.Length; i++)
			{
				array[i] = ofType[i];
			}

			return array;
		}
	}
}
