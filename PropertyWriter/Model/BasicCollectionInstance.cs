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
	class BasicCollectionInstance : Instance
	{
		public BasicCollectionInstance( Type type )
		{
			CollectionValue = new CollectionValue( type );
			CollectionValue.OnValueChanged += () => PropertyChanged.Raise( this, ValueName, FormatedStringName );
		}

		public ObservableCollection<IInstance> Collection
		{
			get { return CollectionValue.Collection; }
		}

		public override object Value
		{
			get { return CollectionValue.Value; }
		}

		public override event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		internal static readonly string ValueName = PropertyName<BasicCollectionInstance>.Get( _ => _.Value );

		private CollectionValue CollectionValue { get; set; }
	}
}
