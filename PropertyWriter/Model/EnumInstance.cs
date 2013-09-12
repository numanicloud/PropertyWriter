using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MvvmHelper;

namespace PropertyWriter.Model
{
	class EnumInstance : Instance
	{
		public EnumInstance( Type type )
		{
			if( !type.IsEnum )
			{
				throw new ArgumentException( "type が列挙型を表す Type クラスではありません。" );
			}

			EnumValues = type.GetEnumValues()
				.Cast<object>()
				.ToArray();

			if( EnumValues.Length != 0 )
			{
				EnumValue = EnumValues[0];
			}
		}

		public object[] EnumValues { get; set; }

		public override object Value
		{
			get { return EnumValue; }
		}

		#region EnumValue

		public object EnumValue
		{
			get { return _EnumValue; }
			set
			{
				_EnumValue = value;
				PropertyChanged.Raise( this, EnumValueName, ValueName, FormatedStringName );
			}
		}

		private object _EnumValue;
		internal static readonly string EnumValueName = PropertyName<EnumInstance>.Get( _ => _.EnumValue );

		#endregion


		public override event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		internal static readonly string ValueName = PropertyName<EnumInstance>.Get( _ => _.Value );
	}
}
