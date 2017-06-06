using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Models.Serialize;
using PropertyWriter.Models.Properties;
using System.Collections.Generic;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Annotation;
using System.Linq;
using PropertyWriter.Models;
using Reactive.Bindings;

namespace PropertyWriterTest.Models
{
	using References = List<(ReferenceByIntProperty reference, int id)>;

	[TestClass]
	public class ModelConverterTest
	{
		public class Hoge
		{
			[PwMember]
			public int X { get; set; }
			[PwMember]
			public float Y { get; set; }
			[PwMember]
			public DayOfWeek Z { get; set; }
			public int W { get; set; }
		}

		public struct Fuga
		{
			[PwMember]
			public int X { get; set; }
			[PwMember]
			public float Y { get; set; }
			[PwMember]
			public DayOfWeek Z { get; set; }
			public int W { get; set; }
		}

		[PwSubtyping]
		public class Subtyping
		{
			[PwMember]
			public int X { get; set; }
		}

		[PwSubtype]
		public class Subtype1
		{
			[PwMember]
			public int Y { get; set; }
		}

		[PwSubtype("Z2")]
		public class Subtype2
		{
			[PwMember]
			public int Z { get; set; }
		}

		[TestMethod]
		public void Intをモデルに読み込むことができる()
		{
			var intModel = new IntProperty();

			Load(intModel, 91);

			intModel.IntValue.Value.Is(91);
		}

		[TestMethod]
		public void Stringをモデルに読み込むことができる()
		{
			var stringModel = new StringProperty(false);

			Load(stringModel, "Muffet");

			stringModel.StringValue.Value.Is("Muffet");
		}

		[TestMethod]
		public void Boolをモデルに読み込むことができる()
		{
			var boolModel = new BoolProperty();

			Load(boolModel, true);

			boolModel.BoolValue.Value.Is(true);
		}

		[TestMethod]
		public void Floatをモデルに読み込むことができる()
		{
			var floatModel = new FloatProperty();

			Load(floatModel, 0.25f);

			floatModel.FloatValue.Value.Is(0.25f);
		}

		[TestMethod]
		public void Enumをモデルに読み込むことができる()
		{
			var enumModel = new EnumProperty(typeof(DayOfWeek));

			Load(enumModel, DayOfWeek.Tuesday);

			enumModel.EnumValue.Value.Is(DayOfWeek.Tuesday);
		}

		[TestMethod]
		public void Classをモデルに読み込むことができる()
		{
			var classModel = new ClassProperty(typeof(Hoge), new PropertyFactory());
			var value = new Hoge
			{
				X = 101,
				Y = 0.25f,
				Z = DayOfWeek.Saturday,
				W = -1
			};

			Load(classModel, value);

			var result = classModel.Value.Value as Hoge;
			result.X.Is(101);
			result.Y.Is(0.25f);
			result.Z.Is(DayOfWeek.Saturday);
			result.W.Is(0);
		}

		[TestMethod]
		public void Structをモデルに読み込むことができる()
		{
			var structModel = new StructProperty(typeof(Fuga), new PropertyFactory());
			var value = new Fuga
			{
				X = 101,
				Y = 0.25f,
				Z = DayOfWeek.Saturday,
				W = -1
			};

			Load(structModel, value);

			var result = (Fuga)structModel.Value.Value;
			result.X.Is(101);
			result.Y.Is(0.25f);
			result.Z.Is(DayOfWeek.Saturday);
			result.W.Is(0);
		}

		[TestMethod]
		public void 変換中にReferenceByIntが来たら参照情報に書き込む()
		{
			var collection = new ComplicateCollectionProperty(typeof(Hoge[]), new PropertyFactory());
			ReferencableMasterInfo source = new ReferencableMasterInfo()
			{
				Collection = collection.Collection.ToReadOnlyReactiveCollection(x => x.Value.Value),
				Type = typeof(Hoge),
			};
			var refModel = new ReferenceByIntProperty(source, "Id");
			var references = new References();

			Load(refModel, 12, references);

			references[0].id.Is(12);
			references[0].reference.Is(refModel);
		}

		[TestMethod]
		public void Subtypeをモデルに書きこむことができる()
		{
			var types = new[] { typeof(Subtype1), typeof(Subtype2) };
			var subtypeModel = new SubtypingProperty(typeof(Subtyping), new PropertyFactory(), types);
			subtypeModel.OnError.Subscribe(x => { throw x; });

			Load(subtypeModel, new Subtype2() { Z = 12 });

			subtypeModel.SelectedType.Value.Type.Is(typeof(Subtype2));
			subtypeModel.SelectedType.Value.Name.Is("Z2");
			var subtype = subtypeModel.Model.Value.Value.Value.IsInstanceOf<Subtype2>();
			subtype.Z.Is(12);
		}

		[TestMethod]
		public void ComplicateCollectionをモデルに書きこむことができる()
		{
			var model = new ComplicateCollectionProperty(typeof(Hoge[]), new PropertyFactory());
			var value = new Hoge[]
			{
				new Hoge { X = 91 },
				new Hoge { X = 82 },
				new Hoge { X = 73 },
			};

			Load(model, value);

			var hoge0 = model.Collection[0].Value.Value.IsInstanceOf<Hoge>();
			hoge0.X.Is(91);
			var hoge1 = model.Collection[1].Value.Value.IsInstanceOf<Hoge>();
			hoge1.X.Is(82);
			var hoge2 = model.Collection[2].Value.Value.IsInstanceOf<Hoge>();
			hoge2.X.Is(73);
		}

		[TestMethod]
		public void BasicCollectionをモデルに書きこむことができる()
		{
			var model = new BasicCollectionProperty(typeof(int[]), new PropertyFactory());
			var value = new int[] { 1, 4, 9};

			Load(model, value);

			var v0 = model.Collection[0].Value.Value.IsInstanceOf<int>();
			v0.Is(1);
			var v1 = model.Collection[1].Value.Value.IsInstanceOf<int>();
			v1.Is(4);
			var v2 = model.Collection[2].Value.Value.IsInstanceOf<int>();
			v2.Is(9);
		}

		[TestMethod]
		public void Int参照コレクションをモデルに書きこむことができる()
		{
			var collection = new ComplicateCollectionProperty(typeof(Hoge[]), new PropertyFactory());
			ReferencableMasterInfo source = new ReferencableMasterInfo()
			{
				Collection = collection.Collection.ToReadOnlyReactiveCollection(x => x.Value.Value),
				Type = typeof(Hoge),
			};
			var model = new ReferenceByIntCollectionProperty(source, "Id", new PropertyFactory());
			var value = new int[] { 12, 13, 14 };
			var references = new References();

			Load(model, value, references);

			var ref0 = model.Collection[0].IsInstanceOf<ReferenceByIntProperty>();
			var ref1 = model.Collection[1].IsInstanceOf<ReferenceByIntProperty>();
			var ref2 = model.Collection[2].IsInstanceOf<ReferenceByIntProperty>();
			references[0].id.Is(12);
			references[0].reference.Is(ref0);
			references[1].id.Is(13);
			references[1].reference.Is(ref1);
			references[2].id.Is(14);
			references[2].reference.Is(ref2);
		}

		private void Load(IPropertyModel model, object value, References references = null)
		{
			var converter = new ModelConverter();
			converter.AsDynamic().LoadValueToModel(model, value, references ?? new References());
		}
		
	}
}
