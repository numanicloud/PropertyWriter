using System;
using PropertyWriter.Models.Properties;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;
using System.Reactive;
using System.Reactive.Linq;

namespace PropertyWriter.ViewModels.Properties
{
	class EnumViewModel : PropertyViewModel<EnumProperty>
	{
        public object[] EnumValues => Property.EnumValues;
		public ReactiveProperty<object> EnumValue => Property.EnumValue;
		public override IObservable<Unit> OnChanged => Property.EnumValue.Select(x => Unit.Default);

		public EnumViewModel(EnumProperty property)
			: base(property)
        {
        }
	}
}
