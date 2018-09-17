using System;
using System.Reactive.Linq;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties
{
    public class StringProperty : PropertyModel
    {
		public bool IsMultiLine { get; }
		public ReactiveProperty<string> StringValue { get; } = new ReactiveProperty<string>("");
        public override ReactiveProperty<object> Value { get; }
		public override Type ValueType => typeof(string);

		public StringProperty(bool isMultiLine)
        {
			IsMultiLine = isMultiLine;
            Value = StringValue.Select(x => (object)x).ToReactiveProperty();
        }

		public override void CopyFrom(IPropertyModel property)
		{
			if (property is StringProperty stringProperty)
			{
				StringValue.Value = stringProperty.StringValue.Value;
			}
		}
	}
}
