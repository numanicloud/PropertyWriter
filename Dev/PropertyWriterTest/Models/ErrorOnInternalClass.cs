using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Annotation;
using PropertyWriter.Models.Properties.Common;
using System.Reflection;
using PropertyWriter.Models;

namespace PropertyWriterTest.Models
{
	[TestClass]
	public class ErrorOnInternalClass
	{
		[PwProject]
		class InternalProject
		{
			[PwMaster]
			public int What { get; set; }
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void OnInternalProjectType()
		{
			var factory = new PropertyFactory();
			var obj = factory.Create(typeof(InternalProject), "Test1");
		}

		[ExpectedException(typeof(ArgumentException))]
		[TestMethod]
		public void OnGetStructure()
		{
			var factory = new PropertyFactory();
			var root = factory.GetStructure(Assembly.GetExecutingAssembly(), typeof(InternalProject), new Project[0]);
		}
	}
}
