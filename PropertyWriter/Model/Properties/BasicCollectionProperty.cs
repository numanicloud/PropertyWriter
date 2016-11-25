using PropertyWriter.Model.Interfaces;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model.Properties
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
