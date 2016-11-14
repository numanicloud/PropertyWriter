using PropertyWriter.Annotation;

namespace RpgData
{
	public class Dungeon
	{
		[PwMember]
		public int Id { get; set; }
		[PwMember]
		public string Name { get; set; }
		[PwMember]
		public string EventMapId { get; set; }
		[PwMember]
		public string BackgroundPath { get; set; }
	}
}
