using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Annotation;
using System.Reflection;
using System.Linq;
using PropertyWriter.Models.Properties;
using PropertyWriter.ViewModels.Properties;
using PropertyWriter.ViewModels.Properties.Common;

namespace PropertyWriterTest.BugFix
{
	[TestClass]
	public class InvalidDataTest
	{
		[PwProject]
		public class InvalidMaster
		{
			[PwMaster("データ")]
			public InvalidData[] Invalids { get; set; }
			[PwMaster]
			public InvalidReferringData[] Refer { get; set; }
		}

		public class InvalidData
		{
			[PwMember]
			public int X { get; set; }
		}

		public class InvalidReferringData
		{
			[PwReferenceMember("Data", nameof(InvalidData.X))]
			public int Refer { get; set; }
		}

		[TestMethod]
		public void InvalidReference()
		{
			var factory = new PropertyWriter.Models.Properties.Common.PropertyFactory();
			var root = factory.GetStructure(Assembly.GetAssembly(typeof(InvalidMaster)), typeof(InvalidMaster), new PropertyWriter.Models.Project[0]);
			var prop = (ComplicateCollectionProperty)root.Structure.Properties.First(x => x.PropertyInfo.Name == nameof(InvalidMaster.Refer));
			var vm = new ComplicateCollectionViewModel(prop, new ViewModelFactory(factory));

			bool isHandled = false;
			vm.OnError.Subscribe(x => isHandled = true);
			root.OnError.Subscribe(x => isHandled = true);
			vm.AddCommand.Execute();
			Assert.IsTrue(isHandled);
		}
	}
}
