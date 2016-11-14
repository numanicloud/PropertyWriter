using PropertyWriter.Annotation;

namespace RpgData
{
	public class Dungeon
	{
		[PwMember]
		public int Id;
		[PwMember]
		public string Name;
		[PwMember]
		public string EventMapId;
		[PwMember]
		public string BackgroundPath;
	}
}
