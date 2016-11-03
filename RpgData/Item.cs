using PropertyWriter.Annotation;

namespace RpgData
{
	[PwMaster]
	public class Item
	{
		[PwMember]
		public int Id;
		[PwMember]
		public string Name;
		[PwMember]
		public int Price;
		[PwMember]
		public string BehaviorId;
		[PwMember]
		public bool IsUsable;
		[PwMember]
		public bool IsUsableInMenu;
		[PwMember]
		public string Brief;
		[PwMember]
		public string FullDescription;
		[PwMember]
		public string EffectId;

		public override string ToString() => $"{Id}: {Name}";
	}
}
