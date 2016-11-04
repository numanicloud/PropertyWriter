using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;
using RpgData.Behaviors;
using RpgData.Passive;

namespace RpgData.Active
{
	[PwSubtype("パッシブ付与")]
	class RaiseStatusBehavior : ActiveBehavior
	{
		[PwMember("パッシブ効果")]
		public PassiveBehavior PassiveEffect { get; set; }

		public override string ToString() => $"パッシブ：{PassiveEffect}";
	}
}
