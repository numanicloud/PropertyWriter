using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Annotation;
using System.Runtime.Serialization;

namespace RpgData
{
	[PwProject]
	[DataContract]
	public class DataBase
	{
		[PwMaster("プレイヤー")]
		[DataMember]
		public PlayersAbility[] Players { get; set; }

		[PwMaster("敵キャラ", "Enemy")]
		[DataMember]
		public EnemiesAbility[] Enemies { get; set; }

		[PwMaster("スキル")]
		[DataMember]
		public Skill[] Skills { get; set; }

		[PwMaster("アイテム", "Item")]
		[DataMember]
		public Item[] Items { get; set; }

		[PwMaster("呪い")]
		[DataMember]
		public Curse[] Curses { get; set; }

		[PwMaster("ダンジョン")]
		[DataMember]
		public Dungeon[] Dungeons { get; set; }
	}
}
