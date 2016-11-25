using System.Reactive.Linq;
using Reactive.Bindings;
using PropertyWriter.Model.Properties;

namespace PropertyWriter.Model.Instance
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
