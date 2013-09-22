using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MvvmHelper;

namespace PropertyWriter.Model
{
	class FloatInstance : Instance
	{
		public override object Value
		{
			get { return FloatValue; }
		}

		#region FloatValue

		public float FloatValue
		{
			get { return _FloatValue; }
			set
			{
				_FloatValue = value;
				PropertyChanged.Raise( this, FloatValueName, ValueName, FormatedStringName );
			}
		}

		private float _FloatValue;
		internal static readonly string FloatValueName = PropertyName<FloatInstance>.Get( _ => _.FloatValue );

		#endregion


		public override event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		internal static readonly string ValueName = PropertyName<FloatInstance>.Get( _ => _.Value );
	}
}
