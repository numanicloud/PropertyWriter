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
			TestCreateInstance<IntInstance>( typeof( int ) );
		}

		[TestMethod]
		public void CreateBoolType()
		{
			TestCreateInstance<BoolInstance>( typeof( bool ) );
		}

		[TestMethod]
		public void CreateStringType()
		{
			TestCreateInstance<StringInstance>( typeof( string ) );
		}

		[TestMethod]
		public void CreateFloatType()
		{
			TestCreateInstance<FloatInstance>( typeof( float ) );
		}

		[TestMethod]
		public void CreateEnumType()
		{
			TestCreateInstance<EnumInstance>( typeof( TestEnum ) );
		}

		[TestMethod]
		public void CreateClassType()
		{
			TestCreateInstance<ClassInstance>( typeof( TestClass ) );
		}

		[TestMethod]
		public void CreateStructType()
		{
			TestCreateInstance<StructInstance>( typeof( TestStruct ) );
		}

		[TestMethod]
		public void CreateBasicCollectionType()
		{
			TestCreateInstance<BasicCollectionInstance>( typeof( IEnumerable<int> ) );
		}

		[TestMethod]
		public void CreateComplicateCollectionType()
		{
			TestCreateInstance<ComplicateCollectionInstance>( typeof( IEnumerable<TestClass> ) );
		}

		private void TestCreateInstance<Expected>( Type type )
		{
			InstanceFactory.Create( type )
				.IsInstanceOf<Expected>();
		}
	}
}
