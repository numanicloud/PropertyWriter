using PropertyWriter.Models.Properties;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;

namespace PropertyWriter.ViewModels.Properties
{
	class FloatViewModel : PropertyViewModel
	{
        private FloatProperty Property { get; }

        public ReactiveProperty<float> FloatValue => Property.FloatValue;
        public override ReactiveProperty<object> Value => Property.Value;

        public FloatViewModel(FloatProperty property)
        {
            Property = property;
        }
	}
}
