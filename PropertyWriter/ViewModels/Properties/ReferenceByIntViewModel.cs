using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace PropertyWriter.ViewModels.Properties
{
	class ReferenceByIntViewModel : PropertyViewModel<ReferenceByIntProperty>
	{
        public ReferencableMasterInfo Source => Property.Source;
		public ReactiveProperty<object> SelectedObject => Property.SelectedObject;
        public ReactiveProperty<int> IntValue => Property.IntValue;
		public override IObservable<Unit> OnChanged => Property.IntValue.Select(x => Unit.Default);

		public ReferenceByIntViewModel(ReferenceByIntProperty property)
			: base(property)
        {
        }
	}
}
