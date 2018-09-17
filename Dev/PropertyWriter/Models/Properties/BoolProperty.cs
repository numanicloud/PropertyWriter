using System;
using System.Reactive.Linq;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties
{
    public class BoolProperty : PropertyModel
    {
        public ReactiveProperty<bool> BoolValue { get; } = new ReactiveProperty<bool>();
        public override ReactiveProperty<object> Value { get; }
		public override Type ValueType => typeof(bool);

		public BoolProperty()
        {
            Value = BoolValue.Select(x => (object)x).ToReactiveProperty();
        }

		public override void CopyFrom(IPropertyModel property)
		{
			if (property is BoolProperty boolProperty)
			{
				boolProperty.BoolValue.Value = BoolValue.Value;
			}
		}
	}
}
