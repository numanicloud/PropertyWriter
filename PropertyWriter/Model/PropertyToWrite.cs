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
	class PropertyToWrite : INotifyPropertyChanged
	{
		public PropertyToWrite( PropertyInfo info )
		{
			this.Info = info;
			Value = Activator.CreateInstance( info.PropertyType );
		}

		public PropertyInfo Info { get; private set; }

		#region Value

		public object Value
		{
			get { return _Value; }
			set
			{
				_Value = value;
				PropertyChanged.Raise( this, ValueName );
			}
		}

		private object _Value;
		internal static readonly string ValueName = PropertyName<PropertyToWrite>.Get( _ => _.Value );

		#endregion


		public event PropertyChangedEventHandler PropertyChanged;
	}
}
