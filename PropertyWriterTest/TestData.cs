using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriterTest
{
	class TestData
	{
		public IEnumerable<int> TestBasic { get; set; }
		public IEnumerable<TestClass> TestComplicate { get; set; }
		public int Int { get; set; }
		public bool Bool { get; set; }
		public float Float { get; set; }
		public string String { get; set; }
		public TestClass Class { get; set; }
		public TestStruct Struct { get; set; }
		public TestEnum Enum { get; set; }
	}
}
