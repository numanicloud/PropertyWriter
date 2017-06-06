using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Annotation;
using PropertyWriter.Models.Properties.Common;
using System.Collections.Generic;
using System.Reflection;
using PropertyWriter.Models.Info;
using PropertyWriter.Models.Properties;
using System.Linq;
using PropertyWriter.Models.Serialize;
using Reactive.Bindings;
using System.Collections.ObjectModel;

namespace PropertyWriterTest.Models
{
	using MasterRepository = Dictionary<string, ReferencableMasterInfo>;

	[TestClass]
	public class MasterBuilderTest
	{
		[PwProject]
		public class Hoge
		{
			[PwMaster("ふが", "Fugas")]
			public Fuga[] Fugas { get; set; }
			[PwMaster]
			public Fuga Fuga { get; set; }
			public Fuga NotMember { get; set; }

			public void Run()
			{
			}
		}
		public class Fuga
		{
			[PwMember]
			public int X { get; set; }
		}

		[PwSubtyping]
		public class Subtyping
		{
			[PwMember]
			public int X { get; set; }
		}

		[PwSubtype]
		public class Subtype1 : Subtyping
		{
			[PwMember]
			public int Y { get; set; }
		}

		[PwSubtype]
		public class Subtype2 : Subtyping
		{
			[PwMember]
			public int Z { get; set; }
		}

		[TestMethod]
		public void プロパティと属性に対応したMasterInfoが生成される()
		{
			PropertyFactory factory = new PropertyFactory();
			var builder = new MasterLoader(factory);
			var info = typeof(Hoge).GetMember(nameof(Hoge.Fugas))[0] as PropertyInfo;
			var attr = new PwMasterAttribute("ふが", "Fugas");

			var master = builder.AsDynamic().GetMasterinfo((info, attr)) as MasterInfo;

			master.Key.Is("Fugas");
			master.Property.Is(info);
			var collection = master.Master.IsInstanceOf<ComplicateCollectionProperty>();
			collection.ElementType.Is(typeof(Fuga));
			collection.Count.Value.Is(0);
			collection.Title.Value.Is("ふが");
			collection.ValueType.Is(typeof(Fuga[]));
		}

		[TestMethod]
		public void MasterInfoをもとに参照情報を生成できる()
		{
			PropertyFactory factory = new PropertyFactory();
			var builder = new MasterLoader(factory);
			var info = new MasterInfo[]
			{
				new MasterInfo("Fugas",
					typeof(Hoge).GetMember(nameof(Hoge.Fugas))[0] as PropertyInfo,
					new ComplicateCollectionProperty(typeof(Fuga[]), factory)),
				new MasterInfo("Fuga",
					typeof(Hoge).GetMember(nameof(Hoge.Fuga))[0] as PropertyInfo,
					new ClassProperty(typeof(Fuga), factory))
			};

			var repo = builder.AsDynamic()
				.GetReferencableMasters(info as IEnumerable<MasterInfo>) as MasterRepository;

			repo.ContainsKey("Fugas").IsTrue();
			repo["Fugas"].Type.Is(typeof(Fuga));
			repo.ContainsKey("Fuga").IsFalse();
		}

		[TestMethod]
		public void マスタープロパティを列挙できる()
		{
			PropertyFactory factory = new PropertyFactory();
			var builder = new MasterLoader(factory);
			var members = typeof(Hoge).GetMembers();

			var masters = builder.AsDynamic()
				.GetMastersProperties(members) as IEnumerable<(PropertyInfo info, PwMasterAttribute attr)>;

			var dic = masters.ToDictionary(x => x.info.Name, x => x);
			dic.ContainsKey(nameof(Hoge.Fugas)).IsTrue();
			dic.ContainsKey(nameof(Hoge.Fuga)).IsTrue();
			dic.ContainsKey(nameof(Hoge.NotMember)).IsFalse();
			dic.ContainsKey(nameof(Hoge.Run)).IsFalse();
			dic[nameof(Hoge.Fugas)].attr.Name.Is("ふが");
			dic[nameof(Hoge.Fuga)].attr.Name.IsNull();
			dic[nameof(Hoge.Fugas)].attr.Key.Is("Fugas");
			dic[nameof(Hoge.Fuga)].attr.Key.IsNull();
		}

		[TestMethod]
		public void サブクラス情報をロードできる()
		{
			PropertyFactory factory = new PropertyFactory();
			var builder = new MasterLoader(factory);
			var domain = new Type[]
			{
				typeof(Subtyping),
				typeof(Subtype1),
				typeof(Subtype2),
				typeof(Hoge),
				typeof(Fuga),
			};

			var subtypes = builder.AsDynamic().GetSubtypes(domain) as Dictionary<Type, Type[]>;

			subtypes.ContainsKey(typeof(Subtyping)).IsTrue();
			subtypes[typeof(Subtyping)].Contains(typeof(Subtype1)).IsTrue();
			subtypes[typeof(Subtyping)].Contains(typeof(Subtype2)).IsTrue();
			subtypes.ContainsKey(typeof(Hoge)).IsFalse();
			subtypes.ContainsKey(typeof(Fuga)).IsFalse();
		}

		[TestMethod]
		public void ReferencableMasterInfoのコレクションの更新が反映される()
		{
			PropertyFactory factory = new PropertyFactory();
			var root = factory.GetStructure(Assembly.GetExecutingAssembly(), 
				typeof(Hoge),
				new PropertyWriter.Models.Project[0]);

			int countB = factory.AsDynamic().loader.Masters["Fugas"].Collection.Count;
			countB.Is(0);

			var fugas = root.Structure.Properties.First(x => x.PropertyInfo.Name == "Fugas")
				.IsInstanceOf<ComplicateCollectionProperty>();
			fugas.AddNewElement();

			int countA = factory.AsDynamic().loader.Masters["Fugas"].Collection.Count;
			countA.Is(1);
		}
	}
}
