using PropertyWriter.Annotation;

namespace RpgData
{
	public class Curse
	{
		[PwMember]
		public int Id { get; set; }
		[PwMember]
		public string Name { get; set; }
		[PwMember]
		public string EffectId { get; set; }
		[PwMember]
		public string Description { get; set; }
		[PwMember]
		public int ItemDropLevel { get; set; }
		[PwMember]
		public int Price { get; set; }
	}
}
