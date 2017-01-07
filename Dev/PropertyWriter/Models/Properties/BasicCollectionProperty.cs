using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties
{
    class BasicCollectionProperty : PropertyModel, ICollectionProperty
    {
        private CollectionHolder collectionValue_;

        public ReadOnlyReactiveCollection<IPropertyModel> Collection { get; }
        public override ReactiveProperty<object> Value { get; }

        public BasicCollectionProperty(Type type, PropertyFactory modelFactory)
        {
            collectionValue_ = new CollectionHolder(type, modelFactory);
            Value = collectionValue_.Value.Cast<object>().ToReactiveProperty();
			collectionValue_.OnError.Subscribe(x => OnErrorSubject.OnNext(x));

			Collection = collectionValue_.Collection.ToReadOnlyReactiveCollection(x => x.model);
		}

        public IPropertyModel AddNewElement()
        {
            return collectionValue_.AddNewElement();
        }

        public void RemoveAt(int index)
        {
            collectionValue_.RemoveAt(index);
        }
    }
}
