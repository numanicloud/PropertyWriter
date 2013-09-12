using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestDll
{
	[DataContract( Namespace = "" )]
	public class Class1
	{
		[DataMember]
		public int X { get; set; }
		[DataMember]
		public bool Boolean { get; set; }

		public override string ToString()
		{
			return string.Format( "({0},{1})", X, Boolean );
		}
	}
}
