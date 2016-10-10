using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	class PropertyToWrite
	{
		public PropertyToWrite(PropertyInfo info)
		{
			this.Info = info;
			Value = new ReactiveProperty<object>
			{
				Value = Activator.CreateInstance(info.PropertyType)
			};
		}

		public PropertyInfo Info { get; private set; }

		public ReactiveProperty<object> Value { get; set; }
	}
}
