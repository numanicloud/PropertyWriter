using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;

namespace RpgData
{
	[PwProject]
	public class DataBase
	{
		[PwMaster("プレイヤー")]
		public PlayersAbility[] Players { get; set; }
		[PwMaster("敵キャラ")]
		public EnemiesAbility[] Enemies { get; set; }
		[PwMaster("スキル")]
		public Skill[] Skills { get; set; }
		[PwMaster("アイテム", "Item")]
		public Item[] Items { get; set; }
		[PwMaster("呪い")]
		public Curse[] Curses { get; set; }
		[PwMaster("ダンジョン")]
		public Dungeon[] Dungeons { get; set; }
	}
}
