using PropertyWriter.Annotation;

namespace RpgData
{
	[PwMinor]
	public class ItemDrop
	{
		[PwReferenceMember(typeof(Item), nameof(Item.Id), "アイテム")]
		public int ItemId;
		[PwMember]
		public int Point;
		[PwMember]
		public int RequiredCurseLevel;

		public override string ToString() => $"{ItemId}, {Point}pt, lv.{RequiredCurseLevel}";
	}
}
