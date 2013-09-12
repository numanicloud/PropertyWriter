using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using MvvmHelper;

namespace PropertyWriter.Model
{
	class StructInstance : Instance
	{
		public StructInstance( Type type )
		{
			if( !type.IsValueType )
			{
				throw new ArgumentException( "type が構造体を表す Type クラスではありません。" );
			}

			StructValue = new StructureValue( type );
			StructValue.OnValueChanged += () => PropertyChanged.Raise( this, ValueName, FormatedStringName );
		}

		public IEnumerable<IInstance> Instances
		{
			get { return StructValue.Instances; }
		}

		public override object Value
		{
			get { return StructValue.Value; }
		}

		private StructureValue StructValue { get; set; }

		public override event PropertyChangedEventHandler PropertyChanged;
		internal static readonly string ValueName = PropertyName<EnumInstance>.Get( _ => _.Value );
	}
}
