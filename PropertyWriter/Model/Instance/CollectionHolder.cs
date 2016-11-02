using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	class CollectionHolder
	{
	    private readonly ModelFactory _modelFactory;

	    public CollectionHolder(Type type, ModelFactory modelFactory)
		{
		    _modelFactory = modelFactory;
		    this.ItemType = type.GenericTypeArguments[0];

			Collection = new ObservableCollection<IPropertyModel>();
		}

		public ReactiveProperty<IEnumerable<object>> Value => Collection.ToCollectionChanged()
			.Select(x => Collection.Cast<object>())
			.ToReactiveProperty();

		public ObservableCollection<IPropertyModel> Collection { get; }

		public void AddNewProperty()
		{
			var instance = _modelFactory.Create(ItemType);
			Collection.Add( instance );
		}

		public void RemoveAt( int index )
		{
			Collection.RemoveAt( index );
		}

		public Type ItemType { get; }
	}
}
