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
		public IPropertyModel Master { get; }

		public MasterInfo(string key, PropertyInfo property, IPropertyModel master)
		{
			Property = property;
			Master = master;
			Key = key ?? Property.Name;
		}
	}
}
