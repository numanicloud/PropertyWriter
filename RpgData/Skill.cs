using PropertyWriter.Annotation;

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
		[PwMember]
		public string BehaviorId;
		[PwMember]
		public string Brief;
		[PwMember]
		public string FullDescription;
		[PwMember]
		public string EffectId;
	}
}
