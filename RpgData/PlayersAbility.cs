using PropertyWriter.Annotation;

namespace RpgData
{
	public class PlayersAbility
	{
		[PwMember]
		public int Id { get; set; }
		[PwMember]
		public string Name { get; set; }
		[PwMember]
		public int MaxHp { get; set; }
		[PwMember]
		public int MaxPain { get; set; }
		[PwMember]
		public Tolerance Tolerance { get; set; }
		[PwMember]
		public string FighterImagePath { get; set; }
		[PwMember]
		public string TumbnailPath { get; set; }
	}
}
