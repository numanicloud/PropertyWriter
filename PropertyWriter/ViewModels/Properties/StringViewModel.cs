using PropertyWriter.Models.Properties;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;

namespace PropertyWriter.ViewModels.Properties
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
