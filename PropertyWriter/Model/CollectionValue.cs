using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace PropertyWriter.Model
{
	class CollectionValue
	{
		public CollectionValue( Type type )
		{
			this.itemType = type.GenericTypeArguments[0];

			var listType = typeof( List<> ).MakeGenericType( this.itemType );
			Value = Activator.CreateInstance( listType ) as IEnumerable<object>;

			Collection = new ObservableCollection<IInstance>();
			Collection.CollectionChanged += Collection_CollectionChanged;
			Collection.ForEach(
				( _, i ) => _.PropertyChanged +=
					( sender, e ) => OnPropertyChanged( i, _.Value ) );
		}

		public IEnumerable<object> Value { get; private set; }

		public ObservableCollection<IInstance> Collection { get; private set; }


		public event Action OnValueChanged;

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

		private void Collection_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
		{
			Value = Collection.Select( _ => _.Value ).ToArray();
			OnValueChanged();
		}

		private void OnPropertyChanged( int index, object newValue )
		{
			Value = Value.Select( ( _, i ) => i == index ? _ : newValue ).ToArray();
			OnValueChanged();
		}
	}
}
