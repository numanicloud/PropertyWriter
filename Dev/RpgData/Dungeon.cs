using PropertyWriter.Annotation;
using System.Runtime.Serialization;

namespace RpgData
{
	[DataContract]
	public class Dungeon
	{
		[PwMember]
		[DataMember]
		public int Id { get; set; }

		[PwMember]
		[DataMember]
		public string Name { get; set; }

		[PwMember]
		[DataMember]
		public string EventMapId { get; set; }

		[PwMember]
		[DataMember]
		public string BackgroundPath { get; set; }
	}
}
