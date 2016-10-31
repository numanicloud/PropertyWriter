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
			var enumProp = new EnumModel( typeof( TestEnum ) );
			enumProp.EnumValues.Is( TestEnum.Red, TestEnum.Green );
			enumProp.Value.Value.Is( TestEnum.Red );
		}

		[TestMethod]
		public void ClassProperties()
		{
			var classProp = new ClassModel( typeof( TestClass ) );
			classProp.Properties.ElementAt( 0 ).Instance.IsInstanceOf<IntModel>();
			classProp.Properties.ElementAt( 1 ).Instance.IsInstanceOf<StringModel>();
		}
	}
}
