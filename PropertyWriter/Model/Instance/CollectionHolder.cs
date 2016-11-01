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
		public CollectionHolder( Type type )
		{
			this.ItemType = type.GenericTypeArguments[0];

			Collection = new ObservableCollection<IPropertyModel>();
		}

		public ReactiveProperty<IEnumerable<object>> Value => Collection.ToCollectionChanged()
			.Select(x => Collection.Cast<object>())
			.ToReactiveProperty();

		public ObservableCollection<IPropertyModel> Collection { get; }

		public void AddNewProperty()
		{
			var instance = InstanceFactory.Create( ItemType );
			Collection.Add( instance );
		}

		public void RemoveAt( int index )
		{
			Collection.RemoveAt( index );
		}

		public Type ItemType { get; }
	}
}
