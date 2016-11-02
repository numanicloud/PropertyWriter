using PropertyWriter.Annotation;

namespace RpgData
{
	[PwMaster]
	public class PlayersAbility
	{
		[PwMember]
		public int Id;
		[PwMember]
		public string Name;
		[PwMember]
		public int MaxHp;
		[PwMember]
		public int MaxPain;
		[PwMember]
		public Tolerance Tolerance;
		[PwMember]
		public string FighterImagePath;
		[PwMember]
		public string TumbnailPath;
	}
}
