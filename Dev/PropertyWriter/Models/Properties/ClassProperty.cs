using System;
using System.Linq;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;
using System.Reactive;

namespace PropertyWriter.Models.Properties
{
    public class ClassProperty : PropertyModel, IStructureProperty
    {
        internal StructureHolder structureValue_;

        public override Type ValueType { get; }
		public override ReactiveProperty<object> Value { get; }
        public IPropertyModel[] Members => structureValue_.Properties.ToArray();
		public IObservable<Unit> OnChanged => structureValue_.ValueChanged;

		public ClassProperty(Type type, PropertyFactory modelFactory)
        {
            ValueType = type;
            if (!type.IsClass)
            {
                throw new ArgumentException("type がクラスを表す Type クラスではありません。");
            }

            structureValue_ = new StructureHolder(type, modelFactory);
            Value = new ReactiveProperty<object>(structureValue_.Value.Value, ReactivePropertyMode.RaiseLatestValueOnSubscribe);

			structureValue_.ValueChanged.Subscribe(
				x => Value.Value = structureValue_.Value.Value);

			structureValue_.OnError.Subscribe(x => OnErrorSubject.OnNext(x));
		}
    }
}
