using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties
{
    public class BasicCollectionProperty : PropertyModel, ICollectionProperty
    {
        public CollectionHolder CollectionValue { get; }
		public override Type ValueType => CollectionValue.Type;
		public ReadOnlyReactiveCollection<IPropertyModel> Collection { get; }
        public override ReactiveProperty<object> Value { get; }

		public ReactiveProperty<int> Count => CollectionValue.Count;

		public BasicCollectionProperty(Type type, PropertyFactory modelFactory)
        {
            CollectionValue = new CollectionHolder(type, modelFactory);
            Value = CollectionValue.Value.Cast<object>().ToReactiveProperty();
			CollectionValue.OnError.Subscribe(x => OnErrorSubject.OnNext(x));

			Collection = CollectionValue.Collection.ToReadOnlyReactiveCollection(x => x.model);
		}

        public IPropertyModel AddNewElement()
        {
            return CollectionValue.AddNewElement();
        }

		public void RemoveElementAt(int index)
		{
			CollectionValue.RemoveAt(index);
		}

		public void Move(int oldIndex, int newIndex)
		{
			CollectionValue.Move(oldIndex, newIndex);
		}

		public void Duplicate(int source, int destination) => CollectionValue.Duplicate(source, destination);

		public override void CopyFrom(IPropertyModel property)
		{
			if (property is BasicCollectionProperty collectionProperty
				&& CollectionValue.ItemType == collectionProperty.CollectionValue.ItemType)
			{
				foreach (var item in collectionProperty.CollectionValue.Collection)
				{
					var clone = AddNewElement();
					clone.CopyFrom(item.model);
				}
			}
		}
	}
}
