using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Models.Properties;
using PropertyWriter.ViewModels.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels.Properties.Common;
using PropertyWriter.Annotation;
using PropertyWriter.Models.Properties.Common;
using System.Linq;

namespace PropertyWriterTest.ViewModels
{
	[TestClass]
	public class OnChangedTest
	{
		[TestMethod]
		public void OnIntChangedTest()
		{
			bool isCalled = false;

			var intProp = new IntProperty();
			var intVm = new IntViewModel(intProp);
			intVm.OnChanged.Subscribe(x => isCalled = true);

			intVm.IntValue.Value = 24;
			isCalled.IsTrue();
		}

		[TestMethod]
		public void OnFloatChangedTest()
		{
			bool isCalled = false;

			var floatProp = new FloatProperty();
			var vm = new FloatViewModel(floatProp);
			vm.OnChanged.Subscribe(x => isCalled = true);

			vm.FloatValue.Value = 1.2f;
			isCalled.IsTrue();
		}

		[TestMethod]
		public void OnBoolChangedTest()
		{
			bool isCalled = false;

			var boolProp = new BoolProperty();
			var vm = new BoolViewModel(boolProp);
			vm.OnChanged.Subscribe(x => isCalled = true);

			vm.BoolValue.Value = true;
			isCalled.IsTrue();
		}

		[TestMethod]
		public void OnStringChangedTest()
		{
			bool isCalled = false;

			var stringProp = new StringProperty(false);
			var vm = new StringViewModel(stringProp);
			vm.OnChanged.Subscribe(x => isCalled = true);

			vm.StringValue.Value = "Hello";
			isCalled.IsTrue();
		}

		enum Hoge
		{
			A, B, C, D
		}

		[TestMethod]
		public void OnEnumChangedTest()
		{
			bool isCalled = false;

			var prop = new EnumProperty(typeof(Hoge));
			var vm = new EnumViewModel(prop);
			vm.OnChanged.Subscribe(x => isCalled = true);

			vm.EnumValue.Value = Hoge.B;
			isCalled.IsTrue();
		}

		class MyClass
		{
			[PwMember]
			public int X { get; set; }
			[PwMember]
			public int Y { get; set; }
			[PwMember]
			public int Z { get; set; }
		}

		[TestMethod]
		public void OnClassChangedTest()
		{
			bool isCalled = false;

			var factory = new PropertyFactory();
			var prop = new ClassProperty(typeof(MyClass), factory);
			var vm = new ClassViewModel(prop, new ViewModelFactory(false));
			vm.OnChanged.Subscribe(x => isCalled = true);

			var memberY = vm.Members.OfType<IntViewModel>()
				.Where(x => x.Property.PropertyInfo.Name == nameof(MyClass.Y))
				.First();
			memberY.IntValue.Value = 99;
			isCalled.IsTrue();

			isCalled = false;
			var memberZ = vm.Members.OfType<IntViewModel>()
				.Where(x => x.Property.PropertyInfo.Name == nameof(MyClass.Z))
				.First();
			memberZ.IntValue.Value = 81;
			isCalled.IsTrue();
		}

		struct MyStruct
		{
			[PwMember]
			public int X { get; set; }
			[PwMember]
			public int Y { get; set; }
			[PwMember]
			public int Z { get; set; }
		}

		[TestMethod]
		public void OnStructChangedTest()
		{
			bool isCalled = false;

			var factory = new PropertyFactory();
			var prop = new StructProperty(typeof(MyStruct), factory);
			var vm = new StructViewModel(prop, new ViewModelFactory(false));
			vm.OnChanged.Subscribe(x => isCalled = true);

			var memberY = vm.Members.OfType<IntViewModel>()
				.Where(x => x.Property.PropertyInfo.Name == nameof(MyStruct.Y))
				.First();
			memberY.IntValue.Value = 99;
			isCalled.IsTrue();

			isCalled = false;
			var memberZ = vm.Members.OfType<IntViewModel>()
				.Where(x => x.Property.PropertyInfo.Name == nameof(MyStruct.Y))
				.First();
			memberZ.IntValue.Value = 81;
			isCalled.IsTrue();
		}

		[PwSubtyping]
		class MyBase
		{
			[PwMember]
			public int X { get; set; }
		}

		[PwSubtype]
		class MyDerived : MyBase
		{
			[PwMember]
			public int Y { get; set; }
		}

		[TestMethod]
		public void OnSubtypeChangedTest()
		{
			bool isCalled = false;

			var factory = new PropertyFactory();
			var prop = new SubtypingProperty(typeof(MyBase), factory, new Type[] { typeof(MyDerived) });
			var vm = new SubtypingViewModel(prop, new ViewModelFactory(false));
			vm.OnChanged.Subscribe(x => isCalled = true);

			prop.SelectedType.Value = prop.AvailableTypes.First();
			isCalled.IsTrue();

			prop.Model.Value.IsInstanceOf<ClassProperty>();
			var members = (prop.Model.Value as ClassProperty).Members;

			isCalled = false;
			var memberX = (IntProperty)members.First(x => x.PropertyInfo.Name == nameof(MyClass.X));
			memberX.IntValue.Value = 99;
			isCalled.IsTrue();

			isCalled = false;
			var memberY = (IntProperty)members.First(x => x.PropertyInfo.Name == nameof(MyClass.Y));
			memberY.IntValue.Value = 81;
			isCalled.IsTrue();
		}

		[TestMethod]
		public void BasicCollectonOnAddingTest()
		{
			bool isCalled = false;
			var factory = new PropertyFactory();
			var prop = new BasicCollectionProperty(typeof(int[]), factory);
			var vm = new BasicCollectionViewModel(prop, new ViewModelFactory(false));
			vm.OnChanged.Subscribe(x => isCalled = true);

			vm.AddCommand.Execute();
			isCalled.IsTrue();
		}

		[TestMethod]
		public void BasicCollectonOnRemovingTest()
		{
			bool isCalled = false;
			var factory = new PropertyFactory();
			var prop = new BasicCollectionProperty(typeof(int[]), factory);
			var vm = new BasicCollectionViewModel(prop, new ViewModelFactory(false));
			vm.OnChanged.Subscribe(x => isCalled = true);
			
			vm.AddCommand.Execute();

			isCalled = false;
			vm.RemoveCommand.Execute(0);
			isCalled.IsTrue();
		}

		[TestMethod]
		public void ComplicateCollectionOnAddingTest()
		{
			bool isCalled = false;
			var factory = new PropertyFactory();
			var prop = new ComplicateCollectionProperty(typeof(MyClass[]), factory);
			var vm = new ComplicateCollectionViewModel(prop, new ViewModelFactory(false));
			vm.OnChanged.Subscribe(x => isCalled = true);

			vm.AddCommand.Execute();
			isCalled.IsTrue();
		}

		[TestMethod]
		public void ComplicateCollectionOnRemovingTest()
		{
			bool isCalled = false;
			var factory = new PropertyFactory();
			var prop = new ComplicateCollectionProperty(typeof(MyClass[]), factory);
			var vm = new ComplicateCollectionViewModel(prop, new ViewModelFactory(false));
			vm.OnChanged.Subscribe(x => isCalled = true);
			
			vm.AddCommand.Execute();

			isCalled = false;
			vm.RemoveCommand.Execute(0);
			isCalled.IsTrue();
		}
	}
}
