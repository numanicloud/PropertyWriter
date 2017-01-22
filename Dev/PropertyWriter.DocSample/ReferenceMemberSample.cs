using PropertyWriter.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.DocSample.ReferenceMember
{
	[PwProject]
	public class Project
	{
		[PwMaster("アイテム", "Items")]
		public Item[] ItemData { get; set; }
		[PwMaster("敵キャラ", "Enemies")]
		public Enemy[] EnemyData { get; set; }
	}

	public class Item
	{
		[PwMember]
		public int Id { get; set; }
		[PwMember]
		public string Name { get; set; }

		public override string ToString() => $"{Id}: {Name}";
	}

	public class Enemy
	{
		[PwMember]
		public string Name { get; set; }
		[PwReferenceMember("Items", nameof(Item.Id))]
		public int DropItemId { get; set; }
	}
}
