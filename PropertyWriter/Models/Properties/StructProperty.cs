using System;
using System.Linq;
using System.Reactive.Linq;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties
{
    class StructProperty : PropertyModel, IStructureProperty
    {
        private StructureHolder StructureValue { get; }

        public Type Type { get; }
        public IPropertyModel[] Members => StructureValue.Properties.ToArray();
        public override ReactiveProperty<object> Value { get; }

        public StructProperty(Type type, PropertyFactory modelFactory)
        {
            Type = type;
            if (!type.IsValueType)
            {
                throw new ArgumentException("type が構造体を表す Type クラスではありません。");
            }

            StructureValue = new StructureHolder(type, modelFactory);

            Value = StructureValue.ValueChanged
                .Select(x => StructureValue.Value.Value)
                .ToReactiveProperty();
        }
    }
}
