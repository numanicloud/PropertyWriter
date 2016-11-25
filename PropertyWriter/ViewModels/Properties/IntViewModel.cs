using PropertyWriter.Models.Properties;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;

namespace PropertyWriter.ViewModels.Properties
{
	class IntViewModel : PropertyViewModel
	{
        private IntProperty Property { get; }

		public ReactiveProperty<int> IntValue => Property.IntValue;
		public override ReactiveProperty<object> Value => Property.Value;

        public IntViewModel(IntProperty property)
        {
            Property = property;
        }
	}
}
