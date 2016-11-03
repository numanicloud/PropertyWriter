using PropertyWriter.Annotation;
using RpgData.Behaviors;

namespace RpgData
{
	[PwMaster]
	public class Skill
	{
		[PwMember]
		public int Id;
		[PwMember]
		public string Name;
		[PwMember]
		public int Pain;
		[PwMember("効果")]
		public ActiveBehavior ActiveEffect;
		[PwMember]
		public string Brief;
		[PwMember]
		public string FullDescription;
		[PwMember]
		public string EffectId;
	}
}
