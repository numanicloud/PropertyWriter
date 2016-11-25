using System.Reactive.Linq;
using Reactive.Bindings;
using PropertyWriter.Model.Properties;

namespace PropertyWriter.Model.Instance
{
	class StringViewModel : PropertyViewModel
	{
        private StringProperty Property { get; }

		public ReactiveProperty<string> StringValue => Property.StringValue;
		public override ReactiveProperty<object> Value => Property.Value;

        public StringViewModel(StringProperty property)
        {
            Property = property;
        }
	}
}
