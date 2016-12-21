using System.Reactive.Linq;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties
{
    class FloatProperty : PropertyModel
    {
        public ReactiveProperty<float> FloatValue { get; } = new ReactiveProperty<float>();
        public override ReactiveProperty<object> Value { get; }

        public FloatProperty()
        {
            Value = FloatValue.Select(x => (object)x).ToReactiveProperty();
        }
    }
}
