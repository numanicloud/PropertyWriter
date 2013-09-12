using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using MvvmHelper;

namespace PropertyWriter.Model
{
	class ClassInstance : Instance
	{
		public ClassInstance( Type type )
		{
			if( !type.IsClass )
			{
				throw new ArgumentException( "type がクラスを表す Type クラスではありません。" );
			}

			ClassValue = new StructureValue( type );
			ClassValue.OnValueChanged += () => PropertyChanged.Raise( this, ValueName, FormatedStringName );
		}

		public IEnumerable<PropertyInstance> Properties
		{
			get { return ClassValue.Properties; }
		}

		public override object Value
		{
			get { return ClassValue.Value; }
		}

		private StructureValue ClassValue { get; set; }

		public override event PropertyChangedEventHandler PropertyChanged;
		internal static readonly string ValueName = PropertyName<EnumInstance>.Get( _ => _.Value );
	}
}
