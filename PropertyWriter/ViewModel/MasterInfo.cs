using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model.Instance;

namespace PropertyWriter.Model
{
	class MasterInfo
	{
		public string Key { get; }
		public PropertyInfo Property { get; }
		public IPropertyViewModel Master { get; }

		public MasterInfo(string key, PropertyInfo property, IPropertyViewModel master)
		{
			Property = property;
			Master = master;
			Key = key ?? Property.Name;
		}
	}
}
