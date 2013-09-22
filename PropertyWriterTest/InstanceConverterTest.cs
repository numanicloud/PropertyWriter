using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Model;

namespace PropertyWriterTest
{
	[TestClass]
	public class InstanceConverterTest
	{
		[TestMethod]
		public void ConvertArray()
		{
			object[] objArray = new object[] { 1, 2, 3, true };

			var result = InstanceConverter.Convert( objArray, typeof( int[] ) );

			result.IsInstanceOf<int[]>();
			( result as int[] ).Is( 1, 2, 3 );
		}

		[TestMethod]
		public void ConvertToIEnumerable()
		{
			object[] objArray = new object[] { 1, 2, 3, true };

			var result = InstanceConverter.Convert( objArray, typeof( IEnumerable<int> ) );

			result.IsInstanceOf<IEnumerable<int>>();
		}
	}
}
