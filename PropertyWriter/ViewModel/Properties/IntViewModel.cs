using System.Reactive.Linq;
using Reactive.Bindings;
using PropertyWriter.Model.Properties;

namespace PropertyWriter.Model.Instance
{
	class IntModel : PropertyViewModel
	{
        private IntProperty Property { get; }

		public ReactiveProperty<int> IntValue => Property.IntValue;
		public override ReactiveProperty<object> Value => Property.Value;

        public IntModel(IntProperty property)
        {
            Property = property;
        }
	}
}
