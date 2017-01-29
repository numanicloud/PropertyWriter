using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Annotation;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.ViewModels.Properties;
using PropertyWriter.ViewModels.Properties.Common;
using System.Linq;
using PropertyWriter.Models.Properties.Interfaces;
using System.Reflection;
using PropertyWriter.Models;

namespace PropertyWriterTest.ViewModels
{
	[TestClass]
	public class OnChangedPropagatingTest
	{
		#region ClassPropagate
		public class Hoge
		{
			[PwMember]
			public Fuga Fuga { get; set; }
		}

		public class Fuga
		{
			[PwMember]
			public Foo Foo { get; set; }
		}

		public class Foo
		{
			[PwMember]
			public int X { get; set; }
		}

		[TestMethod]
		public void ClassPropagateTest()
		{
			bool isCalled = false;
			var prop = new ClassProperty(typeof(Hoge), new PropertyFactory());
			var vm = new ClassViewModel(prop, new ViewModelFactory());
			vm.OnChanged.Subscribe(x => isCalled = true);

			var memberFuga = (ClassProperty)prop.Members.First(x => x.PropertyInfo.Name == nameof(Hoge.Fuga));
			var memberFoo = (ClassProperty)memberFuga.Members.First(x => x.PropertyInfo.Name == nameof(Fuga.Foo));
			var memberX = (IntProperty)memberFoo.Members.First(x => x.PropertyInfo.Name == nameof(Foo.X));

			memberX.IntValue.Value = 999;
			isCalled.IsTrue();
		}
		#endregion

		#region StructPropagate
		public struct Toriel
		{
			[PwMember]
			public Sans Sans { get; set; }
		}
		public struct Sans
		{
			[PwMember]
			public Papyrus Papyrus { get; set; }
		}
		public struct Papyrus
		{
			[PwMember]
			public int X { get; set; }
		}

		[TestMethod]
		public void StructPropagateTest()
		{
			bool isCalled = false;
			var prop = new StructProperty(typeof(Toriel), new PropertyFactory());
			var vm = new StructViewModel(prop, new ViewModelFactory());
			vm.OnChanged.Subscribe(x => isCalled = true);

			var memberSans = (StructProperty)prop.Members.First(x => x.PropertyInfo.Name == nameof(Toriel.Sans));
			var memberPapyrus = (StructProperty)memberSans.Members.First(x => x.PropertyInfo.Name == nameof(Sans.Papyrus));
			var memberX = (IntProperty)memberPapyrus.Members.First(x => x.PropertyInfo.Name == nameof(Papyrus.X));

			memberX.IntValue.Value = 999;
			isCalled.IsTrue();
		}
		#endregion

		#region SubtypingPropagate
		[PwProject]
		public class SubtypeProject
		{
			[PwMaster]
			public A A { get; set; }
		}

		[PwSubtyping]
		public class A
		{
			[PwMember]
			public int X { get; set; }
		}

		[PwSubtype]
		public class AX : A
		{
			[PwMember]
			public B B { get; set; }
		}

		[PwSubtyping]
		public class B
		{
			[PwMember]
			public int Y { get; set; }
		}

		[PwSubtype]
		public class BX : B
		{
			[PwMember]
			public C C { get; set; }
		}

		public class C
		{
			[PwMember]
			public int Z { get; set; }
		}

		[TestMethod]
		public void SubtypingPropagateTest()
		{
			bool isCalled = false;
			var factory = new PropertyFactory();
			factory.GetStructure(Assembly.GetExecutingAssembly(), typeof(SubtypeProject), new Project[0]);

			var prop = factory.Create(typeof(A), "SubtypeA") as SubtypingProperty;
			var vm = new ViewModelFactory().Create(prop, false) as SubtypingViewModel;
			vm.OnChanged.Subscribe(x => isCalled = true);

			T GetMember<T>(IStructureProperty model, string name) => (T)model.Members.First(x => x.PropertyInfo.Name == name);

			prop.SelectedType.Value = prop.AvailableTypes.First();
			var memberB = GetMember<SubtypingProperty>(prop.Model.Value as ClassProperty, nameof(AX.B));

			memberB.SelectedType.Value = memberB.AvailableTypes.First();
			var memberC = GetMember<ClassProperty>(memberB.Model.Value as ClassProperty, nameof(BX.C));

			var memberX = GetMember<IntProperty>(memberC, nameof(C.Z));

			memberX.IntValue.Value = 999;
			isCalled.IsTrue();
		} 
		#endregion
	}
}
