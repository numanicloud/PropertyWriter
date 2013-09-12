using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MvvmHelper;

namespace PropertyWriter.Model
{
	class StringInstance : Instance
	{
		public override object Value
		{
			get { return StringValue; }
		}

		#region StringValue

		public string StringValue
		{
			get { return _StringValue; }
			set
			{
				_StringValue = value;
				PropertyChanged.Raise( this, StringValueName, ValueName, FormatedStringName );
			}
		}

		private string _StringValue;
		internal static readonly string StringValueName = PropertyName<StringInstance>.Get( _ => _.StringValue );

		#endregion


		public override event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		internal static readonly string ValueName = PropertyName<StringInstance>.Get( _ => _.Value );
	}
}
