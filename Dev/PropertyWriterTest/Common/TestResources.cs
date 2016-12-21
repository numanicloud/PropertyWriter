using PropertyWriter.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriterTest
{
    public enum TestEnum
    {
        Red, Green
    }

	public class TestClass
    {
        [PwMember]
        public int X { get; set; }
        [PwMember]
        public string Message { get; set; }
    }

	public interface ITestInterface
    {
        int F();
    }

	public struct TestStruct
    {
        [PwMember]
        public bool B { get; set; }
    }

    [PwProject]
	public class TestProject
    {
        [PwMaster]
        public TestClass[] Classes { get; set; }
    }
}
