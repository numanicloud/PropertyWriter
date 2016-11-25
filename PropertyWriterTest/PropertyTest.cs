using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Model;
using System.Linq;
using System.Collections.Generic;
using PropertyWriter.Model.Instance;

namespace PropertyWriterTest
{
	[TestClass]
	public class PropertyTest
	{
		[TestMethod]
		public void EnumCtor()
		{
			var enumProp = new EnumViewModel(typeof(TestEnum));
			enumProp.EnumValues.Is(TestEnum.Red, TestEnum.Green);
			enumProp.Value.Value.Is(TestEnum.Red);
		}

		[TestMethod]
		public void ClassProperties()
		{
			var classProp = new ClassViewModel(typeof(TestClass), new ModelFactory());
			classProp.Members.ElementAt(0).Model.IsInstanceOf<IntModel>();
			classProp.Members.ElementAt(1).Model.IsInstanceOf<StringModel>();
		}
	}
}
