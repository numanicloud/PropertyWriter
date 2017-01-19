using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Models.Properties;
using System.Linq;
using PropertyWriter.Annotation;
using PropertyWriter.ViewModels.Properties.Common;
using PropertyWriter.ViewModels;

namespace PropertyWriterTest.Models
{
	[TestClass]
	public class RouterTest
	{
		class Hoge
		{
			[PwMember]
			public int X { get; set; }
			[PwMember]
			public Fuga Fuga { get; set; }
		}

		class Fuga
		{
			[PwMember]
			public int Y { get; set; }
			[PwMember]
			public Foo Foo { get; set; }
		}

		class Foo
		{
			[PwMember]
			public int Z { get; set; }
		}

		[TestMethod]
		public void TestMethod1()
		{
			var factory = new PropertyFactory();
			var model = (ClassProperty)factory.Create(typeof(Hoge), "Hoge");

			var xProp = (IntProperty)model.Members.First(x => x.PropertyInfo.Name == "X");
			var fugaProp = (ClassProperty)model.Members.First(x => x.PropertyInfo.Name == "Fuga");
			var yProp = (IntProperty)fugaProp.Members.First(x => x.PropertyInfo.Name == "Y");
			var fooProp = (ClassProperty)fugaProp.Members.First(x => x.PropertyInfo.Name == "Foo");
			var zProp = (IntProperty)fooProp.Members.First(x => x.PropertyInfo.Name == "Z");

			xProp.IntValue.Value = 16;
			yProp.IntValue.Value = 64;
			zProp.IntValue.Value = 256;

			var router = new PropertyRouter(new ViewModelFactory());
			Assert.AreEqual(xProp.IntValue.Value, router.GetIntProperty(model, "X").Value);
			Assert.AreEqual(yProp.IntValue.Value, router.GetIntProperty(model, "Fuga.Y").Value);
			Assert.AreEqual(zProp.IntValue.Value, router.GetIntProperty(model, "Fuga.Foo.Z").Value);
		}
	}
}
