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

        public ObservableCollection<IPropertyModel> Collection => collectionValue_.Collection;
        public override ReactiveProperty<object> Value { get; }

        public BasicCollectionProperty(Type type, PropertyFactory modelFactory)
        {
            collectionValue_ = new CollectionHolder(type, modelFactory);
            Value = collectionValue_.Value.Cast<object>().ToReactiveProperty();
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
