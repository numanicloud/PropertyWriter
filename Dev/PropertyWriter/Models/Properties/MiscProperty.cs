using PropertyWriter.Models.Properties.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties
{
	public class MiscProperty : PropertyModel
	{
		public override ReactiveProperty<object> Value { get; }
		public override Type ValueType { get; }

		public MiscProperty(Type type)
		{
			var val = Activator.CreateInstance(type);
			Value = new ReactiveProperty<object>(val);
			ValueType = type;
		}
	}
}
