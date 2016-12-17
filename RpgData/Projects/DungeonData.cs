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
		[PwMaster("敵キャラ")]
		public EnemyEncount[] Enemies { get; set; }
	}
}
