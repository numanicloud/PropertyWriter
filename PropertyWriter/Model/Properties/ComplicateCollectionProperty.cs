using PropertyWriter.Model.Instance;
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
    class ComplicateCollectionProperty : IPropertyModel
    {
        private CollectionHolder ComplicateCollectionValue { get; }

        public ReactiveProperty<object> Value { get; }
        public ObservableCollection<IPropertyViewModel> Collection => ComplicateCollectionValue.Collection;
        public Type ElementType => ComplicateCollectionValue.ItemType;

        public ComplicateCollectionProperty(Type type, ModelFactory modelFactory)
        {
            ComplicateCollectionValue = new CollectionHolder(type, modelFactory);
            Value = ComplicateCollectionValue.Value
                .Cast<object>()
                .ToReactiveProperty();
        }

        public IPropertyViewModel AddNewElement() => ComplicateCollectionValue.AddNewElement();
    }
}
