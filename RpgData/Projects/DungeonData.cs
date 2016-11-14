using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;

namespace RpgData.Projects
{
	[PwProject]
	public class DungeonData
	{
		[PwMember("敵キャラ")]
		public EnemiesAbility[] Enemies { get; set; }
	}
}
