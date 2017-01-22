using PropertyWriter.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.DocSample.PropertyTitle
{
	[PwProject]
	public class Project
	{
		[PwMaster("アイテム")]
		public Item[] Items { get; set; }
		[PwMaster("敵キャラ")]
		public Enemy[] Enemies { get; set; }
	}

	public class Item
	{
		[PwMember("名前")]
		public string Name { get; set; }
	}

	public class Enemy
	{
		[PwMember("名前")]
		public string Name { get; set; }
		[PwMember("体力")]
		public int HP { get; set; }
	}
}
