using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model.Instance;

namespace PropertyWriter.Model
{
	abstract class InstanceAndMemberInfo
	{
		public InstanceAndMemberInfo(string title, IPropertyModel model)
		{
			Title = title;
			Model = model;
		}

		public IPropertyModel Model { get; }
		public string Title { get; }
		public abstract Type Type { get; }

		public abstract void SetValue(object obj, object value);
		public abstract object GetValue(object obj);
	}
}
