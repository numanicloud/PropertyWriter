using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties
{
    public class ComplicateCollectionProperty : PropertyModel, ICollectionProperty
    {
        private CollectionHolder CollectionValue { get; }

		public override Type ValueType => CollectionValue.Type;
		public override ReactiveProperty<object> Value { get; }
		public ReadOnlyReactiveCollection<IPropertyModel> Collection { get; }
        public Type ElementType => CollectionValue.ItemType;
		public ReactiveProperty<int> Count => CollectionValue.Count;

		public ComplicateCollectionProperty(Type type, PropertyFactory modelFactory)
        {
            CollectionValue = new CollectionHolder(type, modelFactory);
            Value = CollectionValue.Value
                .Cast<object>()
                .ToReactiveProperty();
			CollectionValue.OnError.Subscribe(x => OnErrorSubject.OnNext(x));

			Collection = CollectionValue.Collection.ToReadOnlyReactiveCollection(x => x.model);
		}

        public IPropertyModel AddNewElement() => CollectionValue.AddNewElement();
        public void RemoveElementAt(int index) => CollectionValue.RemoveAt(index);
		public void Move(int oldIndex, int newIndex) => CollectionValue.Move(oldIndex, newIndex);
		public void Duplicate(int source, int destination) => CollectionValue.Duplicate(source, destination);

		public override void CopyFrom(IPropertyModel property)
		{
			if (property is ComplicateCollectionProperty collectionProperty
				&& ElementType == collectionProperty.ElementType)
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
