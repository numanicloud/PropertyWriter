using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;

namespace RpgData.Behaviors
{
	[PwSubtype]
	public class AttackBehavior : ActiveBehavior
	{
		[PwMember("攻撃力")]
		public int Power { get; set; }
		[PwMember("痛み攻撃力")]
		public int Pain { get; set; }
		[PwMember("属性")]
		public DamageAttribute[] Attributes { get; set; }

		public override string ToString() => $"攻 {Power}, 痛 {Pain},\n{string.Join("+", Attributes)} 属性";
	}
}
