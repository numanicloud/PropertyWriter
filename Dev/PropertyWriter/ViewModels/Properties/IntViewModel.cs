using PropertyWriter.Models.Properties;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace PropertyWriter.ViewModels.Properties
{
	class IntViewModel : PropertyViewModel<IntProperty>
	{
		public ReactiveProperty<int> IntValue => Property.IntValue;
		public override IObservable<Unit> OnChanged => Property.IntValue.Select(x => Unit.Default);

		public IntViewModel(IntProperty property)
			: base(property)
        {
        }
	}
}
