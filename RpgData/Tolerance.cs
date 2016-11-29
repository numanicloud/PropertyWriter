using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;
using System.Runtime.Serialization;

namespace RpgData
{
	[DataContract]
	public class Tolerance
	{
		[PwMember]
		[DataMember]
		public int Blow { get; set; }

		[PwMember]
		[DataMember]
		public int Gash { get; set; }

		[PwMember]
		[DataMember]
		public int Burn { get; set; }

		[PwMember]
		[DataMember]
		public int Chill { get; set; }

		[PwMember]
		[DataMember]
		public int Electric { get; set; }

		[PwMember]
		[DataMember]
		public int Primal { get; set; }
	}
}
