using PropertyWriter.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.DocSample.Readability3
{
	[PwProject]
	public class Project
	{
		[PwMaster("プレイヤー")]
		public Player[] Players { get; set; }
	}

	public class Player
	{
		[PwMember]
		public int Id { get; set; }
		[PwMember]
		public string Name { get; set; }
		[PwMember]
		public int Hp { get; set; }
		[PwMember]
		public int Attack { get; set; }

		public override string ToString() => $"{Id}: {Name}";
	}
}
