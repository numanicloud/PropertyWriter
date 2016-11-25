using System.Reactive.Linq;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties
{
    class StringProperty : PropertyModel
    {
        public ReactiveProperty<string> StringValue { get; } = new ReactiveProperty<string>();
        public override ReactiveProperty<object> Value { get; }

        public StringProperty()
        {
            Value = StringValue.Select(x => (object)x).ToReactiveProperty();
        }
    }
}
