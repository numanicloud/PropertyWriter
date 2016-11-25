using PropertyWriter.Model.Interfaces;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model.Properties
{
    class ClassProperty : PropertyModel, IStructureProperty
    {
        private StructureHolder structureValue_;

        public Type Type { get; private set; }
        public override ReactiveProperty<object> Value { get; }
        public IPropertyModel[] Members { get; }
        
        public ClassProperty(Type type, PropertyFactory modelFactory)
        {
            Type = type;
            if (!type.IsClass)
            {
                throw new ArgumentException("type がクラスを表す Type クラスではありません。");
            }

            structureValue_ = new StructureHolder(type, modelFactory);
            Value = new ReactiveProperty<object>(structureValue_.Value.Value, ReactivePropertyMode.RaiseLatestValueOnSubscribe);
            structureValue_.ValueChanged.Subscribe(x => Value.Value = structureValue_.Value.Value);
        }
    }
}
