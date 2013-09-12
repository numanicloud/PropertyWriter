using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;

namespace PropertyWriter.Model
{
	class PropertyInstance
	{
		public PropertyInstance( PropertyInfo info )
		{
			this.PropertyInfo = info;
			Instance = InstanceFactory.Create( info.PropertyType );
			Instance.PropertyChanged += Instance_PropertyChanged;
		}

		public PropertyInfo PropertyInfo { get; private set; }
		public IInstance Instance { get; set; }

		public event Action<PropertyInstance> OnValueChanged;

		void Instance_PropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			if( e.PropertyName != "Value" )
			{
				return;	
			}

			OnValueChanged( this );
		}
	}
}
