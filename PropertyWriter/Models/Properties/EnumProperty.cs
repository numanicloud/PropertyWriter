using System;
using System.Linq;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties
{
    class EnumProperty : PropertyModel
    {
        public Type Type { get; private set; }
        public object[] EnumValues { get; private set; }
        public ReactiveProperty<object> EnumValue { get; set; } = new ReactiveProperty<object>();
        public override ReactiveProperty<object> Value => EnumValue;

        public EnumProperty(Type type)
        {
            Type = type;
            if (!type.IsEnum)
            {
                throw new ArgumentException("type が列挙型を表す Type クラスではありません。");
            }

            EnumValues = type.GetEnumValues()
                .Cast<object>()
                .ToArray();

            if (EnumValues.Length != 0)
            {
                EnumValue.Value = EnumValues[0];
            }
        }
    }
}
