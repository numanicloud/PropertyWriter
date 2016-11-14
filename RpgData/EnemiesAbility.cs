using PropertyWriter.Annotation;

namespace RpgData
{
	public class EnemiesAbility
	{
		[PwMember(name:"ID")]
		public int Id;
		[PwMember("名前")]
		public string Name;
		[PwMember("最大HP")]
		public int MaxHp;
		[PwMember("痛み耐久力")]
		public int MaxPain;
		[PwMember("痛みゲージ数")]
		public int PainToleranceTimes;
		[PwMember("属性耐性")]
		public Tolerance Tolerance;
		[PwMember("AI ID")]
		public string AiId;
		[PwMember("画像パス")]
		public string ImagePath;

		[PwMember("ドロップアイテム")]
		public ItemDrop[] ItemDrop;
		[PwReferenceMember(typeof(Item), nameof(Item.Id), "討伐ドロップ")]
		public int ItemIdOnDefeated;

		public override string ToString() => $"{Id}: {Name}";
	}
}
