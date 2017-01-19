using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Models.Properties;

namespace PropertyWriterTest.Models
{
	[TestClass]
	public class OnChangedTest
	{
		[TestMethod]
		public void OnIntChangedTest()
		{
			bool intValueChanged = false;
			bool valueChanged = false;

			var intProp = new IntProperty();
			intProp.IntValue.Subscribe(x => intValueChanged = true);
			intProp.Value.Subscribe(x => valueChanged = true);

			intProp.IntValue.Value = 12;

			intValueChanged.IsTrue();
			valueChanged.IsTrue();
		}
	}
}
