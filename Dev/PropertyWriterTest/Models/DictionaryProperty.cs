using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PropertyWriter.Annotation;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Models.Properties;
using System.Linq;

namespace PropertyWriterTest.Models
{
	[TestClass]
	public class DictionaryProperty
	{
		public class Hoge
		{
			[PwMember]
			public Dictionary<string, int> Ints { get; set; }
		}

		[TestMethod]
		public void DictionaryPropertyTest()
		{
			var factory = new PropertyFactory();
			var model = factory.Create(typeof(Hoge), "Root");
			
			model.IsInstanceOf<ClassProperty>();
			var classModel = model as ClassProperty;
			var dicModel = classModel.Members.First(x => x.PropertyInfo.Name == nameof(Hoge.Ints));

			dicModel.IsInstanceOf<ComplicateCollectionProperty>();
			var collectionModel = dicModel as ComplicateCollectionProperty;
			var prop = collectionModel.AddNewElement();

			Assert.AreEqual(typeof(KeyValuePair<string, int>), prop.ValueType);
			prop.IsInstanceOf<StructProperty>();
			var kvpModel = prop as StructProperty;
			//kvpModel.Members.First(x => x.PropertyInfo.Name == "Key").Value.Value = "Hash";
			//kvpModel.Members.First(x => x.PropertyInfo.Name == "Value").Value.Value = 123;
		}
	}
}
