using PropertyWriter.Models.Properties;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;

namespace PropertyWriter.ViewModels.Properties
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
