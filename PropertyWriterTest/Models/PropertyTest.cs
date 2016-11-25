using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Common;

namespace PropertyWriterTest
{
	[TestClass]
	public class PropertyTest
	{
		[TestMethod]
		public void EnumCtor()
		{
			var enumProp = new EnumProperty(typeof(TestEnum));
			enumProp.EnumValues.Is(TestEnum.Red, TestEnum.Green);
			enumProp.Value.Value.Is(TestEnum.Red);
		}

		[TestMethod]
		public void ClassProperties()
		{
			var classProp = new ClassProperty(typeof(TestClass), new PropertyFactory());
            classProp.Members.Length.Is(2);
			classProp.Members.ElementAt(0).IsInstanceOf<IntProperty>();
			classProp.Members.ElementAt(1).IsInstanceOf<StringProperty>();
		}

        [TestMethod]
        public void StructPropertyTest()
        {
            var property = new StructProperty(typeof(TestStruct), new PropertyFactory());
            property.Members.Length.Is(1);
            property.Members[0].IsInstanceOf<BoolProperty>();
        }
	}
}
