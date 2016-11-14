using PropertyWriter.Annotation;

namespace RpgData
{
	public class Curse
	{
		[PwMember]
		public int Id;
		[PwMember]
		public string Name;
		[PwMember]
		public string EffectId;
		[PwMember]
		public string Description;
		[PwMember]
		public int ItemDropLevel;
		[PwMember]
		public int Price;
	}
}
