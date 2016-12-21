using PropertyWriter.Models.Properties;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace PropertyWriter.ViewModels.Properties
{
	class StringViewModel : PropertyViewModel<StringProperty>
	{
		public ReactiveProperty<string> StringValue => Property.StringValue;
		public override IObservable<Unit> OnChanged => Property.StringValue.Select(x => Unit.Default);

		public StringViewModel(StringProperty property)
			: base(property)
        {
        }
	}
}
