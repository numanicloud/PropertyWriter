using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Model;

namespace PropertyWriterTest
{
	#region TestType
	enum TestEnum
	{
		Red, Green
	}

	class TestClass
	{
		public int X { get; set; }
		public string Message { get; set; }
	}

	interface TestInterface
	{
		int F();
	}

	struct TestStruct
	{
		public bool B { get; set; }
	}
	#endregion

	[TestClass]
	public class TypeParserTest
	{
		[TestMethod]
		public void ParseIntType()
		{
			var type = typeof( int );

			var result = TypeRecognizer.ParseType( type );

			Assert.AreEqual( PropertyKind.Integer, result );
		}

		[TestMethod]
		public void ParseBoolType()
		{
			var type = typeof( bool );

			var result = TypeRecognizer.ParseType( type );

			Assert.AreEqual( PropertyKind.Boolean, result );
		}

		[TestMethod]
		public void ParseStringType()
		{
			TestParseType<string>( PropertyKind.String );
		}

		[TestMethod]
		public void ParseFloatType()
		{
			TestParseType<float>( PropertyKind.Float );
		}

		[TestMethod]
		public void ParseEnumType()
		{
			TestParseType<TestEnum>( PropertyKind.Enum );
		}

		[TestMethod]
		public void ParseClassType()
		{
			TestParseType<TestClass>( PropertyKind.Class );
		}

		[TestMethod]
		public void ParseInterfaceType()
		{
			TestParseType<TestInterface>( PropertyKind.Class );
		}

		[TestMethod]
		public void ParseStructType()
		{
			TestParseType<TestStruct>( PropertyKind.Struct );
		}

		[TestMethod]
		public void ParseBasicCollectionType()
		{
			TestParseType<IEnumerable<int>>( PropertyKind.BasicCollection );
		}

		[TestMethod]
		public void ParseComplecateCollectionType()
		{
			TestParseType<IEnumerable<TestClass>>( PropertyKind.ComplicateCollection );
			TestParseType<IEnumerable<TestStruct>>( PropertyKind.ComplicateCollection );
		}

		private void TestParseType<TypeForParse>( PropertyKind expected )
		{
			var type = typeof( TypeForParse );

			var actual = TypeRecognizer.ParseType( type );

			Assert.AreEqual( expected, actual );
		}
	}
}
