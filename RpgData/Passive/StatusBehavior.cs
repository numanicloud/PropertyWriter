using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;
using System.Runtime.Serialization;

namespace RpgData.Passive
{
	[PwSubtyping]
	[DataContract]
	public class PassiveBehavior
	{
		[PwMember("名前")]
		[DataMember]
		public string Name { get; set; }
		[PwMember("説明")]
		[DataMember]
		public string Description { get; set; }
	}
}
