using PropertyWriter.Annotation;

namespace RpgData
{
	public class Item
	{
		[PwMember]
		public int Id { get; set; }
		[PwMember]
		public string Name { get; set; }
		[PwMember]
		public int Price { get; set; }
		[PwMember]
		public string BehaviorId { get; set; }
		[PwMember]
		public bool IsUsable { get; set; }
		[PwMember]
		public bool IsUsableInMenu { get; set; }
		[PwMember]
		public string Brief { get; set; }
		[PwMember]
		public string FullDescription { get; set; }
		[PwMember]
		public string EffectId { get; set; }

		public override string ToString() => $"{Id}: {Name}";
	}
}
