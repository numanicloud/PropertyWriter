using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;
using RpgData.Behaviors;
using RpgData.Passive;
using System.Runtime.Serialization;

namespace RpgData.Active
{
	[PwSubtype("パッシブ付与")]
	[DataContract]
	[KnownType(typeof(PoisonStatus))]
	[KnownType(typeof(LazyPainStatus))]
	class RaiseStatusBehavior : ActiveBehavior
	{
		[PwMember("パッシブ効果")]
		[DataMember]
		public PassiveBehavior PassiveEffect { get; set; }

		public override string ToString() => $"パッシブ：{PassiveEffect}";
	}
}
