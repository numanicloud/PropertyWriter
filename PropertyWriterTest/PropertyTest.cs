using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Model;
using System.Linq;
using System.Collections.Generic;
using PropertyWriter.Model.Instance;
using PropertyWriter.Model.Properties;

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
			classProp.Members.ElementAt(0).IsInstanceOf<IntProperty>();
			classProp.Members.ElementAt(1).IsInstanceOf<StringProperty>();
		}
	}
}
