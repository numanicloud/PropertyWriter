using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;

namespace PropertyWriter
{
	class DllLoader
	{
		public static Type[] LoadDataTypes( string dllPath )
		{
			return Assembly.LoadFrom( dllPath )
				.GetTypes()
				.Where( _ => _.GetCustomAttributes( typeof( DataContractAttribute ) ).Any() )
				.ToArray();
		}

		public static PropertyInfo[] LoadProperties( Type type )
		{
			return type.GetProperties()
				.Where( _ => _.GetCustomAttributes( typeof( DataMemberAttribute ) ).Any() )
				.ToArray();
		}
	}
}
