using System;
using System.Linq;
using Reactive.Bindings;
using PropertyWriter.Model.Properties;

namespace PropertyWriter.Model.Instance
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
