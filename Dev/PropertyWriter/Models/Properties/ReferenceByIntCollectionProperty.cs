using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Models.Properties
{
	public class ReferenceByIntCollectionProperty : PropertyModel, ICollectionProperty
	{
		private ReferencableMasterInfo Source { get; }
		private string IdPropertyName { get; }
		private CollectionHolder CollectionValue { get; }

		public override Type ValueType => CollectionValue.Type;
		public override ReactiveProperty<object> Value { get; }
		public ReadOnlyReactiveCollection<IPropertyModel> Collection { get; }
		public Type ElementType => CollectionValue.ItemType;

		public ReactiveProperty<int> Count => CollectionValue.Count;

		public ReferenceByIntCollectionProperty(ReferencableMasterInfo source, string idPropertyName, PropertyFactory factory)
		{
			Source = source;
			IdPropertyName = idPropertyName;
			CollectionValue = new CollectionHolder(typeof(int[]), factory);
			Value = CollectionValue.Value
				.Cast<object>()
				.ToReactiveProperty();
			CollectionValue.OnError.Subscribe(x => OnErrorSubject.OnNext(x));

			Collection = CollectionValue.Collection.ToReadOnlyReactiveCollection(x => x.model);
		}

		public IPropertyModel AddNewElement()
		{
			var model = new ReferenceByIntProperty(Source, IdPropertyName)
			{
				Title = { Value = "Element" }
			};
			CollectionValue.AddElement(model);
			return model;
		}
		public void RemoveElementAt(int index) => CollectionValue.RemoveAt(index);
		public void Move(int oldIndex, int newIndex) => CollectionValue.Move(oldIndex, newIndex);
	}
}
