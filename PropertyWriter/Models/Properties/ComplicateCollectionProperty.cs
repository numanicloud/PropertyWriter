using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties
{
    class ComplicateCollectionProperty : PropertyModel, ICollectionProperty
    {
        private CollectionHolder CollectionValue { get; }

        public override ReactiveProperty<object> Value { get; }
        public ObservableCollection<IPropertyModel> Collection => CollectionValue.Collection;
        public Type ElementType => CollectionValue.ItemType;

        public ComplicateCollectionProperty(Type type, PropertyFactory modelFactory)
        {
            CollectionValue = new CollectionHolder(type, modelFactory);
            Value = CollectionValue.Value
                .Cast<object>()
                .ToReactiveProperty();
        }

        public IPropertyModel AddNewElement() => CollectionValue.AddNewElement();
        public void RemoveElementAt(int index) => CollectionValue.RemoveAt(index);
    }
}
