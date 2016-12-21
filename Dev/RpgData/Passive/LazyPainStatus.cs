using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;
using System.Runtime.Serialization;

namespace RpgData.Passive
{
	[PwSubtype]
	[DataContract]
	public class LazyPainStatus : PassiveBehavior
	{
		[PwMember("痛み")]
		[DataMember]
		public int PainAmount { get; set; }
		[PwMember("属性")]
		[DataMember]
		public DamageAttribute Attribute { get; set; }

		public override string ToString() => $"{Attribute} 属性, {PainAmount} 痛み";
	}
}
