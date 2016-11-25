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
    class BasicCollectionProperty : IPropertyModel
    {
        private CollectionHolder collectionValue_;

        public ObservableCollection<Instance.IPropertyViewModel> Collection => collectionValue_.Collection;
        public ReactiveProperty<object> Value { get; }

        public BasicCollectionProperty(Type type, ModelFactory modelFactory)
        {
            collectionValue_ = new CollectionHolder(type, modelFactory);
            Value = collectionValue_.Value.Cast<object>().ToReactiveProperty();
        }

        public Instance.IPropertyViewModel AddNewElement()
        {
            return collectionValue_.AddNewElement();
        }

        public void RemoveAt(int index)
        {
            collectionValue_.RemoveAt(index);
        }
    }
}
