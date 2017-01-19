using System;
using System.Reactive.Linq;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties
{
    public class IntProperty : PropertyModel
    {
        public ReactiveProperty<int> IntValue { get; } = new ReactiveProperty<int>();
        public override ReactiveProperty<object> Value { get; }
		public override Type ValueType => typeof(int);

		public IntProperty()
        {
            Value = IntValue.Select(x => (object)x).ToReactiveProperty();
        }
    }
}
