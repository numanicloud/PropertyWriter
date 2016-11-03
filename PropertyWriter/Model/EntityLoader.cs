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
			return types.Select(x =>
			{
				var attr = x.GetCustomAttribute<PwMasterAttribute>();
				if (attr != null)
				{
					return MasterInfo.ForMaster(x, modelFactory, attr.Name);
				}
				return null;
			}).Where(x => x != null)
				.ToArray();
		}

	    public static MasterInfo[] LoadGlobals(Type[] types, ModelFactory modelFactory)
        {

			return types.Select(x =>
			{
				var attr = x.GetCustomAttribute<PwGlobalAttribute>();
				if(attr != null)
				{
					return MasterInfo.ForGlobal(x, modelFactory, attr.Name);
				}
				return null;
			}).Where(x => x != null)
				.ToArray();
        }

        public static IEnumerable<InstanceAndMemberInfo> LoadMembers(Type type, ModelFactory modelFactory)
        {
            var properties = type.GetProperties()
				.Select(x => MakeModel(x, modelFactory))
				.Where(x => x != null);
            var fields = type.GetFields()
				.Select(x => MakeModel(x, modelFactory))
				.Where(x => x != null);

            return properties.Cast<InstanceAndMemberInfo>().Concat(fields).ToArray();
        }

		private static InstanceAndPropertyInfo MakeModel(PropertyInfo info, ModelFactory modelFactory)
		{
			var memberAttr = info.GetCustomAttribute<PwMemberAttribute>();
			if (memberAttr != null)
			{
				return new InstanceAndPropertyInfo(info, modelFactory.Create(info.PropertyType), memberAttr.Name);
			}

			var attr = info.GetCustomAttribute<PwReferenceMemberAttribute>();
			if (attr != null)
            {
                return new InstanceAndPropertyInfo(
					info,
					modelFactory.CreateReference(info.PropertyType, attr.TargetType, attr.IdFieldName),
					attr.Name);
			}

			return null;
		}

	    private static InstanceAndFieldInfo MakeModel(FieldInfo info, ModelFactory modelFactory)
		{
			var memberAttr = info.GetCustomAttribute<PwMemberAttribute>();
			if (memberAttr != null)
	        {
                return new InstanceAndFieldInfo(info, modelFactory.Create(info.FieldType), memberAttr.Name);
            }

	        var attr = info.GetCustomAttribute<PwReferenceMemberAttribute>();
	        if (attr != null)
            {
                return new InstanceAndFieldInfo(
					info,
					modelFactory.CreateReference(info.FieldType, attr.TargetType, attr.IdFieldName),
					attr.Name);
            }

	        return null;
	    }
	}
}
