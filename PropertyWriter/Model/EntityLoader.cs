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
			var properties = type.GetProperties();
			var fields = type.GetFields();

			var generalProps = properties.Where(IsAnnotatedMember<PwMemberAttribute>).ToArray();
			var refProps = properties.Except(generalProps)
				.Where(IsAnnotatedMember<PwReferenceMemberAttribute>)
				.Select(x =>
				{
					var attr = x.GetCustomAttribute<PwReferenceMemberAttribute>();
					return InstanceAndPropertyInfo.ForReference(x, attr.TargetType, attr.IdFieldName);
				});

			var generalFields = fields.Where(IsAnnotatedMember<PwMemberAttribute>).ToArray();
			var refFields = fields.Except(generalFields)
				.Where(IsAnnotatedMember<PwReferenceMemberAttribute>)
				.Select(x =>
				{
					var attr = x.GetCustomAttribute<PwReferenceMemberAttribute>();
					return InstanceAndFieldInfo.ForReferenceMember(x, attr.TargetType, attr.IdFieldName);
				});

			return generalProps.Select(InstanceAndPropertyInfo.ForMember)
				.Cast<InstanceAndMemberInfo>()
				.Concat(generalFields.Select(InstanceAndFieldInfo.ForMember))
				.Concat(refProps)
				.Concat(refFields)
				.ToArray();
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
