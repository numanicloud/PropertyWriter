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
	public class PoisonStatus : PassiveBehavior
	{
		[PwMember("ダメージ量")]
		[DataMember]
		public int Power { get; set; }

		public override string ToString() => $"毒ダメージ {Power}";
	}
}
