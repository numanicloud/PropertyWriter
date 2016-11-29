using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Annotation;

namespace PropertyWriterTest
{
	[TestClass]
	public class PropertyFactoryTest
	{
		public class ReferenceClass
		{
			[PwMember]
			public int Id { get; set; }
			[PwReferenceMember("LocalClass", nameof(Id))]
			public int IdRef { get; set; }
		}

		[PwProject]
		public class LocalProject
		{
			[PwMaster(key:"LocalClass")]
			public ReferenceClass[] Classes { get; set; }
		}

        #region Create
        [TestMethod]
        public void CreateIntType()
        {
            TestCreateInstance<IntProperty>(typeof(int));
        }

        [TestMethod]
        public void CreateBoolType()
        {
            TestCreateInstance<BoolProperty>(typeof(bool));
        }

        [TestMethod]
        public void CreateStringType()
        {
            TestCreateInstance<StringProperty>(typeof(string));
        }

        [TestMethod]
        public void CreateFloatType()
        {
            TestCreateInstance<FloatProperty>(typeof(float));
        }

        [TestMethod]
        public void CreateEnumType()
        {
            TestCreateInstance<EnumProperty>(typeof(TestEnum));
        }

        [TestMethod]
        public void CreateClassType()
        {
            TestCreateInstance<ClassProperty>(typeof(TestClass));
        }

        [TestMethod]
        public void CreateStructType()
        {
            TestCreateInstance<StructProperty>(typeof(TestStruct));
        }

        [TestMethod]
        public void CreateBasicCollectionType()
        {
            TestCreateInstance<BasicCollectionProperty>(typeof(IEnumerable<int>));
        }

        [TestMethod]
        public void CreateComplicateCollectionType()
        {
            TestCreateInstance<ComplicateCollectionProperty>(typeof(IEnumerable<TestClass>));
        }

        private void TestCreateInstance<Expected>(Type type)
        {
            var factory = new PropertyFactory();
            factory.Create(type, "Test").IsInstanceOf<Expected>();
        }
		#endregion

		[TestMethod]
		public void GetStructureTest()
		{
			var factory = new PropertyFactory();
			var root = factory.GetStructure(typeof(PropertyFactoryTest).Assembly, typeof(LocalProject));

			root.Type.Is(typeof(LocalProject));

			var properties = root.Structure.Properties.ToArray();
			properties.Length.Is(1);
			properties[0].IsInstanceOf<ComplicateCollectionProperty>();

			var collectionProp = (ComplicateCollectionProperty)properties[0];
			collectionProp.Collection.Count.Is(0);
			collectionProp.ElementType.Is(typeof(ReferenceClass));
		}

		[TestMethod]
		public void ReferenceTest()
		{
			var factory = new PropertyFactory();
			var root = factory.GetStructure(typeof(PropertyFactoryTest).Assembly, typeof(LocalProject));
			var properties = root.Structure.Properties.ToArray();
			var collectionProp = (ComplicateCollectionProperty)properties[0];

			var model = collectionProp.AddNewElement();
			model.IsInstanceOf<ClassProperty>();

			var prop = (ClassProperty)model;
			prop.Type.Is(typeof(ReferenceClass));
			prop.Members.Length.Is(2);
			prop.Members[0].IsInstanceOf<IntProperty>();
			prop.Members[1].IsInstanceOf<ReferenceByIntProperty>();

			var refInt = (ReferenceByIntProperty)prop.Members[1];
			refInt.Source.Type.Is(typeof(ReferenceClass));
		}
    }
}
