using System.Reactive.Linq;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties
{
    class StringProperty : PropertyModel
    {
		public bool IsMultiLine { get; }
		public ReactiveProperty<string> StringValue { get; } = new ReactiveProperty<string>();
        public override ReactiveProperty<object> Value { get; }

        public StringProperty(bool isMultiLine)
        {
			IsMultiLine = isMultiLine;
            Value = StringValue.Select(x => (object)x).ToReactiveProperty();
        }
    }
}
