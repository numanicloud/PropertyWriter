using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Model;
using System.Linq;
using System.Collections.Generic;

namespace PropertyWriterTest
{
	[TestClass]
	public class PropertyTest
	{
		[TestMethod]
		public void EnumCtor()
		{
			var enumProp = new EnumInstance( typeof( TestEnum ) );
			enumProp.EnumValues.Is( TestEnum.Red, TestEnum.Green );
			enumProp.Value.Is( TestEnum.Red );
		}

		[TestMethod]
		public void ClassProperties()
		{
			var classProp = new ClassInstance( typeof( TestClass ) );
			classProp.Properties.ElementAt( 0 ).Instance.IsInstanceOf<IntInstance>();
			classProp.Properties.ElementAt( 1 ).Instance.IsInstanceOf<StringInstance>();
		}
	}
}
