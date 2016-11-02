using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PropertyWriter.Annotation;
using PropertyWriter.Model.Instance;

namespace PropertyWriter.Model
{
	class EntityLoader
	{
		public static MasterInfo[] LoadMasters(Type[] types, ModelFactory modelFactory)
		{
			return types.Where(Helpers.IsAnnotatedType<PwMasterAttribute>)
				.Select(x => MasterInfo.ForMaster(x, modelFactory))
                .ToArray();
		}

	    public static MasterInfo[] LoadGlobals(Type[] types, ModelFactory modelFactory)
        {
            return types.Where(Helpers.IsAnnotatedType<PwGlobalAttribute>)
                .Select(x => MasterInfo.ForGlobal(x, modelFactory))
                .ToArray();
        }

        public static IEnumerable<InstanceAndMemberInfo> LoadMembers(Type type, ModelFactory modelFactory)
        {
            var properties = type.GetProperties().Select(x => MakeModel(x, modelFactory));
            var fields = type.GetFields().Select(x => MakeModel(x, modelFactory));

            return properties.Cast<InstanceAndMemberInfo>().Concat(fields).ToArray();
        }

		private static InstanceAndPropertyInfo MakeModel(PropertyInfo info, ModelFactory modelFactory)
		{
			if (Helpers.IsAnnotatedMember<PwMemberAttribute>(info))
			{
				return new InstanceAndPropertyInfo(info, modelFactory.Create(info.PropertyType));
			}

			var attr = info.GetCustomAttribute<PwReferenceMemberAttribute>();
			if (attr != null)
            {
                return new InstanceAndPropertyInfo(info, modelFactory.CreateReference(info.PropertyType, attr.TargetType, attr.IdFieldName));
			}

			return null;
		}

	    private static InstanceAndFieldInfo MakeModel(FieldInfo info, ModelFactory modelFactory)
	    {
	        if (Helpers.IsAnnotatedMember<PwMemberAttribute>(info))
	        {
                return new InstanceAndFieldInfo(info, modelFactory.Create(info.FieldType));
            }

	        var attr = info.GetCustomAttribute<PwReferenceMemberAttribute>();
	        if (attr != null)
            {
                return new InstanceAndFieldInfo(info, modelFactory.CreateReference(info.FieldType, attr.TargetType, attr.IdFieldName));
            }

	        return null;
	    }
	}
}
