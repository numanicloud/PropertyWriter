using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Model;
using System.Linq;

namespace PropertyWriterTest
{
	[TestClass]
	public class PropertyFactoryTest
	{
		[TestMethod]
		public void CreateIntType()
		{
			TestCreateInstance<IntModel>( typeof( int ) );
		}

		[TestMethod]
		public void CreateBoolType()
		{
			TestCreateInstance<BoolModel>( typeof( bool ) );
		}

		[TestMethod]
		public void CreateStringType()
		{
			TestCreateInstance<StringModel>( typeof( string ) );
		}

		[TestMethod]
		public void CreateFloatType()
		{
			TestCreateInstance<FloatModel>( typeof( float ) );
		}

		[TestMethod]
		public void CreateEnumType()
		{
			TestCreateInstance<EnumModel>( typeof( TestEnum ) );
		}

		[TestMethod]
		public void CreateClassType()
		{
			TestCreateInstance<ClassModel>( typeof( TestClass ) );
		}

		[TestMethod]
		public void CreateStructType()
		{
			TestCreateInstance<StructModel>( typeof( TestStruct ) );
		}

		[TestMethod]
		public void CreateBasicCollectionType()
		{
			TestCreateInstance<BasicCollectionModel>( typeof( IEnumerable<int> ) );
		}

		[TestMethod]
		public void CreateComplicateCollectionType()
		{
			TestCreateInstance<ComplicateCollectionModel>( typeof( IEnumerable<TestClass> ) );
		}

		private void TestCreateInstance<Expected>( Type type )
		{
			InstanceFactory.Create( type )
				.IsInstanceOf<Expected>();
		}
	}
}
