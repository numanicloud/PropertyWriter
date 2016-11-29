using PropertyWriter.Annotation;
using System.Runtime.Serialization;

namespace RpgData
{
	[DataContract]
	public class Curse
	{
		[PwMember]
		[DataMember]
		public int Id { get; set; }

		[PwMember]
		[DataMember]
		public string Name { get; set; }

		[PwMember]
		[DataMember]
		public string EffectId { get; set; }

		[PwMember]
		[DataMember]
		public string Description { get; set; }

		[PwMember]
		[DataMember]
		public int ItemDropLevel { get; set; }

		[PwMember]
		[DataMember]
		public int Price { get; set; }
	}
}
