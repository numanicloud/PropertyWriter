using PropertyWriter.Annotation;
using RpgData.Behaviors;

namespace RpgData
{
	public class Skill
	{
		[PwMember]
		public int Id { get; set; }
		[PwMember]
		public string Name { get; set; }
		[PwMember]
		public int Pain { get; set; }
		[PwMember("効果")]
		public ActiveBehavior ActiveEffect { get; set; }
		[PwMember]
		public string Brief { get; set; }
		[PwMember]
		public string FullDescription { get; set; }
		[PwMember]
		public string EffectId { get; set; }

		public override string ToString() => $"{Id}: {Name}";
	}
}
