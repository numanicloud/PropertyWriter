using System;
using System.Reactive;
using PropertyWriter.Models.Properties;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;
using System.Reactive.Linq;

namespace PropertyWriter.ViewModels.Properties
{
	public class BoolViewModel : PropertyViewModel<BoolProperty>
	{
		public ReactiveProperty<bool> BoolValue => Property.BoolValue;
		public override IObservable<Unit> OnChanged => Property.BoolValue.Select(x => Unit.Default);

		public BoolViewModel(BoolProperty property)
			: base(property)
        {
            Property = property;
        }
	}
}
