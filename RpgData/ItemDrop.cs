using PropertyWriter.Annotation;

namespace RpgData
{
	public class ItemDrop
	{
		[PwReferenceMember("Item", nameof(Item.Id), "アイテム")]
		public int ItemId { get; set; }
		[PwMember]
		public int Point { get; set; }
		[PwMember]
		public int RequiredCurseLevel { get; set; }

		public override string ToString() => $"{ItemId}, {Point}pt, lv.{RequiredCurseLevel}";
	}
}
