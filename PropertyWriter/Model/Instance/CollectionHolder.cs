using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	class CollectionHolder
	{
		public CollectionHolder( Type type )
		{
			this.itemType = type.GenericTypeArguments[0];

			Collection = new ObservableCollection<IPropertyModel>();
		}

		public ReactiveProperty<IEnumerable<object>> Value => Collection.ToCollectionChanged()
			.Select(x => Collection.Cast<object>())
			.ToReactiveProperty();

		public ObservableCollection<IPropertyModel> Collection { get; }

		public void AddNewProperty()
		{
			var instance = InstanceFactory.Create( itemType );
			Collection.Add( instance );
		}

		public void RemoveAt( int index )
		{
			Collection.RemoveAt( index );
		}

		private Type itemType { get; set; }
	}
}
