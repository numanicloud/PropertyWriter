using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PropertyWriter.Annotation;

namespace PropertyWriter.Model
{
	class EntityLoader
	{
		public static MasterInfo[] LoadDataTypes(string dllPath)
		{
			var types = Assembly.LoadFrom(dllPath)
				.GetTypes();
			var globals = types.Where(IsAnnotatedType<PwGlobalAttribute>)
				.Select(MasterInfo.ForGlobal);
			var masters = types.Where(IsAnnotatedType<PwMasterAttribute>)
				.Select(MasterInfo.ForMaster);
			return globals.Concat(masters).ToArray();
		}

		private static bool IsAnnotatedType<TAttribute>(Type type)
		{
			return type.CustomAttributes.Any(x => x.AttributeType == typeof(TAttribute));
		}

		public static InstanceAndMemberInfo[] LoadMembers(Type type)
		{
			var properties = type.GetProperties()
				.Where(IsAnnotatedMember<PwMemberAttribute>)
				.Select(InstanceAndPropertyInfo.ForMember);
			var fields = type.GetFields()
				.Where(IsAnnotatedMember<PwMemberAttribute>)
				.Select(InstanceAndFieldInfo.ForMember);
			return properties.Cast<InstanceAndMemberInfo>().Concat(fields).ToArray();
		}

		private static bool IsAnnotatedMember<TAttribute>(MemberInfo member)
		{
			return member.CustomAttributes.Any(x => x.AttributeType == typeof (TAttribute));
		}

		public static PropertyInfo[] LoadProperties(Type type)
		{
			return type.GetProperties()
				//.Where( _ => _.GetCustomAttributes( typeof( DataMemberAttribute ) ).Any() )
				.ToArray();
		}
	}
}
