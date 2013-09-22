using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MvvmHelper;

namespace PropertyWriter.Model
{
	class BoolInstance : Instance
	{
		public override object Value
		{
			get { return BoolValue; }
		}

		#region BoolValue

		public bool BoolValue
		{
			get { return _BoolValue; }
			set
			{
				_BoolValue = value;
				PropertyChanged.Raise( this, BoolValueName, ValueName, FormatedStringName );
			}
		}

		private bool _BoolValue;
		internal static readonly string BoolValueName = PropertyName<BoolInstance>.Get( _ => _.BoolValue );

		#endregion

		public override event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		internal static readonly string ValueName = PropertyName<BoolInstance>.Get( _ => _.Value );
	}
}
