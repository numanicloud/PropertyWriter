using System.Reactive.Linq;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties
{
    class BoolProperty : PropertyModel
    {
        public ReactiveProperty<bool> BoolValue { get; } = new ReactiveProperty<bool>();
        public override ReactiveProperty<object> Value { get; }

        public BoolProperty()
        {
            Value = BoolValue.Select(x => (object)x).ToReactiveProperty();
        }
    }
}
