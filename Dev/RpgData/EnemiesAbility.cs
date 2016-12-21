using PropertyWriter.Annotation;
using System.Runtime.Serialization;

namespace RpgData
{
	[DataContract]
	public class EnemiesAbility
	{
		[PwMember(name:"ID")]
		[DataMember]
		public int Id { get; set; }

		[PwMember("名前")]
		[DataMember]
		public string Name { get; set; }

		[PwMember("最大HP")]
		[DataMember]
		public int MaxHp { get; set; }

		[PwMember("痛み耐久力")]
		[DataMember]
		public int MaxPain { get; set; }

		[PwMember("痛みゲージ数")]
		[DataMember]
		public int PainToleranceTimes { get; set; }

		[PwMember("属性耐性")]
		[DataMember]
		public Tolerance Tolerance { get; set; }

		[PwMember("AI ID")]
		[DataMember]
		public string AiId { get; set; }

		[PwMember("画像パス")]
		[DataMember]
		public string ImagePath { get; set; }

		[PwMember("ドロップアイテム")]
		[DataMember]
		public ItemDrop[] ItemDrop { get; set; }

		[PwReferenceMember("Item", nameof(Item.Id), "討伐ドロップ")]
		[DataMember]
		public int ItemIdOnDefeated { get; set; }

		public override string ToString() => $"{Id}: {Name}";
	}
}
