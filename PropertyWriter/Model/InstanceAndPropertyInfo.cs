using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;

namespace PropertyWriter.Model
{
	class InstanceAndPropertyInfo
	{
		public InstanceAndPropertyInfo(PropertyInfo info)
		{
			this.PropertyInfo = info;
			Instance = InstanceFactory.Create(info.PropertyType);
		}

		public PropertyInfo PropertyInfo { get; private set; }
		public IPropertyModel Instance { get; set; }
	}
}
