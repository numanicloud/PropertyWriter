using System.Reactive.Linq;
using Reactive.Bindings;
using PropertyWriter.Model.Properties;

namespace PropertyWriter.Model.Instance
{
	class BoolViewModel : PropertyViewModel
	{
        private BoolProperty Property { get; }

        public BoolViewModel(BoolProperty property)
        {
            Property = property;
        }

        public ReactiveProperty<bool> BoolValue => Property.BoolValue;
		public override ReactiveProperty<object> Value => Property.Value;
	}
}
