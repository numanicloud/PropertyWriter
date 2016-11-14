using PropertyWriter.Annotation;

namespace RpgData
{
	public class EnemiesAbility
	{
		[PwMember(name:"ID")]
		public int Id { get; set; }
		[PwMember("名前")]
		public string Name { get; set; }
		[PwMember("最大HP")]
		public int MaxHp { get; set; }
		[PwMember("痛み耐久力")]
		public int MaxPain { get; set; }
		[PwMember("痛みゲージ数")]
		public int PainToleranceTimes { get; set; }
		[PwMember("属性耐性")]
		public Tolerance Tolerance { get; set; }
		[PwMember("AI ID")]
		public string AiId { get; set; }
		[PwMember("画像パス")]
		public string ImagePath { get; set; }

		[PwMember("ドロップアイテム")]
		public ItemDrop[] ItemDrop { get; set; }
		[PwReferenceMember("Item", nameof(Item.Id), "討伐ドロップ")]
		public int ItemIdOnDefeated { get; set; }

		public override string ToString() => $"{Id}: {Name}";
	}
}
