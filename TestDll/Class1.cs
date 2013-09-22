using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestDll
{
	public enum Enum1
	{
		Red, Blue, Yellow
	}

	[DataContract( Namespace = "" )]
	public class Class1
	{
		[DataMember]
		public int X { get; set; }
		[DataMember]
		public bool Boolean { get; set; }
		[DataMember]
		public string Message { get; set; }
		[DataMember]
		public float Single { get; set; }
		[DataMember]
		public Enum1 Enum1 { get; set; }
		[DataMember]
		public IEnumerable<int> IntCollection { get; set; }
		public int NonEdit { get; set; }

		public override string ToString()
		{
			return string.Format( "({0},{1},{2})", X, Boolean, Enum1 );
		}
	}
}
