using PropertyWriter.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriterTest
{
	class TestData
	{
        [PwMember]
		public IEnumerable<int> TestBasic { get; set; }
        [PwMember]
        public IEnumerable<TestClass> TestComplicate { get; set; }
        [PwMember]
        public int Int { get; set; }
        [PwMember]
        public bool Bool { get; set; }
        [PwMember]
        public float Float { get; set; }
        [PwMember]
        public string String { get; set; }
        [PwMember]
        public TestClass Class { get; set; }
        [PwMember]
        public TestStruct Struct { get; set; }
        [PwMember]
        public TestEnum Enum { get; set; }
	}
}
