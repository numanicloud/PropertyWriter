using PropertyWriter.Annotation;

namespace RpgData
{
	[PwMinor]
	public class ItemDrop
	{
		[PwMember]
		public Item Item;
		[PwMember]
		public int Point;
		[PwMember]
		public int RequiredCurseLevel;
	}
}
