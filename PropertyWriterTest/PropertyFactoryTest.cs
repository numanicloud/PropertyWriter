using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Model;
using System.Linq;
using PropertyWriter.Model.Instance;

namespace PropertyWriterTest
{
	[TestClass]
	public class PropertyFactoryTest
	{
		[TestMethod]
		public void CreateIntType()
		{
			TestCreateInstance<IntModel>(typeof(int));
		}

		[TestMethod]
		public void CreateBoolType()
		{
			TestCreateInstance<BoolViewModel>(typeof(bool));
		}

		[TestMethod]
		public void CreateStringType()
		{
			TestCreateInstance<StringModel>(typeof(string));
		}

		[TestMethod]
		public void CreateFloatType()
		{
			TestCreateInstance<FloatViewModel>(typeof(float));
		}

		[TestMethod]
		public void CreateEnumType()
		{
			TestCreateInstance<EnumViewModel>(typeof(TestEnum));
		}

		[TestMethod]
		public void CreateClassType()
		{
			TestCreateInstance<ClassViewModel>(typeof(TestClass));
		}

		[TestMethod]
		public void CreateStructType()
		{
			TestCreateInstance<StructModel>(typeof(TestStruct));
		}

		[TestMethod]
		public void CreateBasicCollectionType()
		{
			TestCreateInstance<BasicCollectionViewModel>(typeof(IEnumerable<int>));
		}

		[TestMethod]
		public void CreateComplicateCollectionType()
		{
			TestCreateInstance<ComplicateCollectionViewModel>(typeof(IEnumerable<TestClass>));
		}

		private void TestCreateInstance<Expected>(Type type)
		{
			var factory = new ModelFactory();
			factory.Create(type, "Test").IsInstanceOf<Expected>();
		}
	}
}
