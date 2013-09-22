using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MvvmHelper;

namespace PropertyWriter.Model
{
	class IntInstance : Instance
	{
		public override object Value
		{
			get { return IntValue; }
		}

		#region IntValue

		public int IntValue
		{
			get { return _IntValue; }
			set
			{
				_IntValue = value;
				PropertyChanged.Raise( this, IntValueName, ValueName, FormatedStringName );
			}
		}

		private int _IntValue;
		internal static readonly string IntValueName = PropertyName<IntInstance>.Get( _ => _.IntValue );

		#endregion


		public override event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		internal static readonly string ValueName = PropertyName<IntInstance>.Get( _ => _.Value );
	}
}
