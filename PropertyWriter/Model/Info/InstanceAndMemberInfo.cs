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
		public InstanceAndMemberInfo(string title, IPropertyViewModel model)
		{
			Model = model;
		}

		public IPropertyViewModel Model { get; }
		public abstract string MemberName { get; }

		public abstract void SetValue(object obj, object value);
		public abstract object GetValue(object obj);
	}
}
