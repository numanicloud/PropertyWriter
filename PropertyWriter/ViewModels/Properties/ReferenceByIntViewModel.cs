using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;

namespace PropertyWriter.ViewModels.Properties
{
	class ReferenceByIntViewModel : PropertyViewModel
	{
        private ReferenceByIntProperty Property { get; }

        public ReferencableMasterInfo Source => Property.Source;
		public ReactiveProperty<object> SelectedObject => Property.SelectedObject;
        public ReactiveProperty<int> IntValue => Property.IntValue;
        public override ReactiveProperty<object> Value => Property.Value;

        public ReferenceByIntViewModel(ReferenceByIntProperty property)
        {
            Property = property;
        }
	}
}
