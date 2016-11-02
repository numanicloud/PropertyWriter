using PropertyWriter.Annotation;

namespace RpgData
{
	[PwMaster]
	public class EnemiesAbility
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
		public int PainToleranceTimes;
		[PwMember]
		public Tolerance Tolerance;
		[PwMember]
		public string AiId;
		[PwMember]
		public string ImagePath;

		[PwMember]
		public ItemDrop[] ItemDrop;
		[PwMember]
		public Item ItemOnDefeated;
	}
}
