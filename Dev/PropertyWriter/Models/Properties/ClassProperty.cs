using System;
using System.Linq;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;
using System.Reactive;

namespace PropertyWriter.Models.Properties
{
    class ClassProperty : PropertyModel, IStructureProperty
    {
        private StructureHolder structureValue_;

        public Type Type { get; private set; }
		public override ReactiveProperty<object> Value { get; }
        public IPropertyModel[] Members => structureValue_.Properties.ToArray();
		public IObservable<Unit> OnChanged => structureValue_.ValueChanged;

		public ClassProperty(Type type, PropertyFactory modelFactory)
        {
            Type = type;
            if (!type.IsClass)
            {
                throw new ArgumentException("type がクラスを表す Type クラスではありません。");
            }

            structureValue_ = new StructureHolder(type, modelFactory);
            Value = new ReactiveProperty<object>(structureValue_.Value.Value, ReactivePropertyMode.RaiseLatestValueOnSubscribe);

			structureValue_.ValueChanged.Subscribe(
				x => Value.Value = structureValue_.Value.Value);
		}
    }
}
