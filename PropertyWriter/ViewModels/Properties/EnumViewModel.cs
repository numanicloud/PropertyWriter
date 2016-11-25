using System;
using PropertyWriter.Models.Properties;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;

namespace PropertyWriter.ViewModels.Properties
{
	class EnumViewModel : PropertyViewModel
	{
        private EnumProperty Property { get; }

        public Type Type => Property.Type;
        public object[] EnumValues => Property.EnumValues;
		public ReactiveProperty<object> EnumValue => Property.EnumValue;
		public override ReactiveProperty<object> Value => EnumValue;

        public EnumViewModel(EnumProperty property)
        {
            Property = property;
        }
	}
}
