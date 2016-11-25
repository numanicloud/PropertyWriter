using PropertyWriter.Model.Interfaces;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model.Properties
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
