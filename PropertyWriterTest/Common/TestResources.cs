using PropertyWriter.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriterTest
{
    enum TestEnum
    {
        Red, Green
    }

    class TestClass
    {
        [PwMember]
        public int X { get; set; }
        [PwMember]
        public string Message { get; set; }
    }

    interface ITestInterface
    {
        int F();
    }

    struct TestStruct
    {
        [PwMember]
        public bool B { get; set; }
    }

    [PwProject]
    class TestProject
    {
        [PwMaster]
        public TestClass[] Classes { get; set; }
    }
}
