using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Annotation;
using PropertyWriter.Models.Properties;
using PropertyWriter.ViewModels.Properties;
using PropertyWriter.ViewModels.Properties.Common;
using System.Reactive.Subjects;
using System.Reactive;
using PropertyWriter.ViewModels;

namespace PropertyWriterTest.ViewModels
{
	[TestClass]
	public class StructureHolderTest
	{
		public class Hoge
		{
			[PwMember]
			public int X { get; set; }
		}

		public class Fuga
		{
			[PwMember]
			public Hoge Hoge { get; set; }
		}

		public ClassViewModel ClassVm { get; set; }
		public PropertyRouter Router { get; set; }

		[TestInitialize]
		public void Initialize()
		{
			var propFactory = new PropertyFactory();
			var classProp = new ClassProperty(typeof(Fuga), propFactory);
			var vmFactory = new ViewModelFactory(propFactory);
			ClassVm = new ClassViewModel(classProp, vmFactory);
			Router = new PropertyRouter(vmFactory);
		}

		[TestMethod]
		public void Editコマンドを実行するとShowDetailイベントを発行する()
		{
			bool detailShown = false;
			ClassVm.ShowDetail.Subscribe(x => detailShown = true);

			ClassVm.EditCommand.Execute();

			detailShown.IsTrue();
		}

		[TestMethod]
		public void ShowDetailイベントを発行したメンバーをCloseUpに設定する()
		{
			var memberVm = ClassVm.Members[0].IsInstanceOf<ClassViewModel>();
			var subject = (Subject<Unit>)memberVm.ShowDetail;

			subject.OnNext(Unit.Default);

			ClassVm.PropertyClosedUp.Value.Is(memberVm);
		}

		[TestMethod]
		public void メンバーのOnChangedが発行されるとクラスもOnChangedを発行する()
		{
			bool isCalled = false;
			var memberHoge = ClassVm.Members[0].IsInstanceOf<ClassViewModel>();
			var memberX = memberHoge.Members[0].IsInstanceOf<IntViewModel>();
			ClassVm.OnChanged.Subscribe(x => isCalled = true);

			memberX.IntValue.Value = 99;

			isCalled.IsTrue();
		}
	}
}
