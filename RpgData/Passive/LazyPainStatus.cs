using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;

namespace RpgData.Passive
{
	[PwSubtype]
	public class LazyPainStatus : PassiveBehavior
	{
		[PwMember("痛み")]
		public int PainAmount { get; set; }
		[PwMember("属性")]
		public DamageAttribute Attribute { get; set; }

		public override string ToString() => $"{Attribute} 属性, {PainAmount} 痛み";
	}
}
