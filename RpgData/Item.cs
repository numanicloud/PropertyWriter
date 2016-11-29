using PropertyWriter.Annotation;
using System.Runtime.Serialization;

namespace RpgData
{
	[DataContract]
	public class Item
	{
		[PwMember]
		[DataMember]
		public int Id { get; set; }

		[PwMember]
		[DataMember]
		public string Name { get; set; }

		[PwMember]
		[DataMember]
		public int Price { get; set; }

		[PwMember]
		[DataMember]
		public string BehaviorId { get; set; }

		[PwMember]
		[DataMember]
		public bool IsUsable { get; set; }

		[PwMember]
		[DataMember]
		public bool IsUsableInMenu { get; set; }

		[PwMember]
		[DataMember]
		public string Brief { get; set; }

		[PwMember]
		[DataMember]
		public string FullDescription { get; set; }

		[PwMember]
		[DataMember]
		public string EffectId { get; set; }

		public override string ToString() => $"{Id}: {Name}";
	}
}
