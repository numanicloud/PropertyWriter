using PropertyWriter.Annotation;
using RpgData.Active;
using RpgData.Behaviors;
using System.Runtime.Serialization;

namespace RpgData
{
	[DataContract]
	[KnownType(typeof(AttackBehavior))]
	[KnownType(typeof(RaiseStatusBehavior))]
	public class Skill
	{
		[PwMember]
		[DataMember]
		public int Id { get; set; }

		[PwMember]
		[DataMember]
		public string Name { get; set; }

		[PwMember]
		[DataMember]
		public int Pain { get; set; }

		[PwMember("効果")]
		[DataMember]
		public ActiveBehavior ActiveEffect { get; set; }

		[PwMember]
		[DataMember]
		public string Brief { get; set; }

		[PwMember]
		[DataMember]
		public string FullDescription { get; set; }

		[PwMember]
		[DataMember]
		public string EffectId { get; set; }

		public override string ToString() => $"{Id}: {Name}";
	}
}
