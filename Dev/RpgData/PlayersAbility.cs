using PropertyWriter.Annotation;
using Reactive.Bindings;
using System.Runtime.Serialization;

namespace RpgData
{
	[DataContract]
	public class PlayersAbility
	{
		[PwMember]
		[DataMember]
		public int Id { get; set; }

		[PwMember]
		[DataMember]
		public string Name { get; set; }

		[PwMember]
		[DataMember]
		public int MaxHp { get; set; }

		[PwMember]
		[DataMember]
		public int MaxPain { get; set; }

		[PwMember]
		[DataMember]
		public Tolerance Tolerance { get; set; }

		[PwMember]
		[DataMember]
		public string FighterImagePath { get; set; }

		[PwMember]
		[DataMember]
		public string TumbnailPath { get; set; }
	}
}
