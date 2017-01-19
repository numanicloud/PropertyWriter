using PropertyWriter.Models.Properties;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace PropertyWriter.ViewModels.Properties
{
	public class FloatViewModel : PropertyViewModel<FloatProperty>
	{
        public ReactiveProperty<float> FloatValue => Property.FloatValue;
		public override IObservable<Unit> OnChanged => Property.FloatValue.Select(x => Unit.Default);

		public FloatViewModel(FloatProperty property)
			: base(property)
        {
        }
	}
}
