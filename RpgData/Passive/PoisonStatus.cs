using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;

namespace RpgData.Passive
{
	[PwSubtype]
	public class PoisonStatus : PassiveBehavior
	{
		[PwMember("ダメージ量")]
		public int Power { get; set; }

		public override string ToString() => $"毒ダメージ {Power}";
	}
}
