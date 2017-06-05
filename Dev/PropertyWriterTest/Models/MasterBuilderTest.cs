using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Annotation;
using PropertyWriter.Models.Properties.Common;
using System.Collections.Generic;

namespace PropertyWriterTest.Models
{
	[TestClass]
	public class MasterBuilderTest
	{
		[PwProject]
		public class Hoge
		{
			[PwMaster("Fugas")]
			public Fuga[] Fugas { get; set; }
			[PwMaster]
			public Fuga Fuga { get; set; }
		}
		public class Fuga
		{
			[PwMember]
			public int X { get; set; }
		}
	}
}
