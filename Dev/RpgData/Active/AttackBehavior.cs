using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;
using System.Runtime.Serialization;

namespace RpgData.Behaviors
{
	[PwSubtype("攻撃")]
	[DataContract]
	public class AttackBehavior : ActiveBehavior
	{
		[PwMember("攻撃力")]
		[DataMember]
		public int Power { get; set; }
		[PwMember("痛み攻撃力")]
		[DataMember]
		public int Pain { get; set; }
		[PwMember("属性")]
		[DataMember]
		public DamageAttribute[] Attributes { get; set; }

		public override string ToString() => $"攻 {Power}, 痛 {Pain},\n{string.Join("+", Attributes)} 属性";
	}
}
