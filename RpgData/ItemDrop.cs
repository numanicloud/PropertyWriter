using PropertyWriter.Annotation;
using System.Runtime.Serialization;

namespace RpgData
{
	[DataContract]
	public class ItemDrop
	{
		[PwReferenceMember("Item", nameof(Item.Id), "アイテム")]
		[DataMember]
		public int ItemId { get; set; }

		[PwMember]
		[DataMember]
		public int Point { get; set; }

		[PwMember]
		[DataMember]
		public int RequiredCurseLevel { get; set; }

		public override string ToString() => $"{ItemId}, {Point}pt, lv.{RequiredCurseLevel}";
	}
}
