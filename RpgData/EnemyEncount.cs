using PropertyWriter.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RpgData
{
	[DataContract]
	public class EnemyEncount
	{
		[DataMember]
		[PwMember]
		public int Weight { get; set; }

		[DataMember]
		[PwReferenceMember("DataBase.Enemy", nameof(EnemiesAbility.Id), "敵")]
		public int EnemyId { get; set; }
	}
}
