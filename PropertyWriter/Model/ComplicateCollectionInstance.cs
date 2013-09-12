using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using MvvmHelper;

namespace PropertyWriter.Model
{
	class ComplicateCollectionInstance : Instance
	{
		public ComplicateCollectionInstance( Type type )
		{
			ComplicateCollectionValue = new CollectionValue( type );
			ComplicateCollectionValue.OnValueChanged += () => PropertyChanged.Raise( this, ValueName, FormatedStringName );
		}


		public ObservableCollection<IInstance> Collection
		{
			get { return ComplicateCollectionValue.Collection; }
		}
		public override object Value
		{
			get { return ComplicateCollectionValue.Value; }
		}

		public void AddNewProperty()
		{
			ComplicateCollectionValue.AddNewProperty();
		}

		public void RemoveAt( int index )
		{
			ComplicateCollectionValue.RemoveAt( index );
		}

		public override event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		internal static readonly string ValueName = PropertyName<ComplicateCollectionInstance>.Get( _ => _.Value );

		private CollectionValue ComplicateCollectionValue { get; set; }
	}
}
