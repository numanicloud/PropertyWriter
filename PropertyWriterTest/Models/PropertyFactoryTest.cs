using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Common;

namespace PropertyWriterTest
{
	[TestClass]
	public class PropertyFactoryTest
	{
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
        
    }
}
