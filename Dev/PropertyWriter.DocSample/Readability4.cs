using PropertyWriter.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.DocSample.Readability4
{
	[PwProject]
	public class Project
	{
		[PwMaster("プレイヤー")]
		public Player[] Players { get; set; }
	}

	public class Player
	{
		[PwMember("プレイヤーID")]
		public int Id { get; set; }
		[PwMember("名前")]
		public string Name { get; set; }
		[PwMember("体力")]
		public int Hp { get; set; }
		[PwMember("攻撃力")]
		public int Attack { get; set; }

		public override string ToString() => $"{Id}: {Name}";
	}
}
