using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;

namespace RpgData
{
	public class Tolerance
	{
		[PwMember]
		public int Blow { get; set; }
		[PwMember]
		public int Gash { get; set; }
		[PwMember]
		public int Burn { get; set; }
		[PwMember]
		public int Chill { get; set; }
		[PwMember]
		public int Electric { get; set; }
		[PwMember]
		public int Primal { get; set; }
	}
}
