using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels.Properties.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.ViewModels.Properties.Extensibility
{
	/// <summary>
	/// ステートフルなビュー モデル ファクトリ。
	/// </summary>
	public class PropertyCompounder
	{
		IPropertyModel model;
		ViewModelRouter router;

		public bool UsePlugin { get; set; }
		public IObservable<Unit> CompoundOnChangedObservable { get; private set; }

		public PropertyCompounder(IPropertyModel model, ViewModelFactory factory)
		{
			UsePlugin = true;
			this.model = model;
			router = new ViewModelRouter(factory);
			CompoundOnChangedObservable = Observable.Empty<Unit>();
		}

		public TViewModel CreateViewModel<TViewModel>(string route) where TViewModel : IPropertyViewModel
		{
			var vm = (TViewModel)router.CreateViewModel(model, route, UsePlugin);
			CompoundOnChangedObservable = CompoundOnChangedObservable.Merge(vm.OnChanged);
			return vm;
		}
		public IntViewModel CreateIntViewModel(string route) => CreateViewModel<IntViewModel>(route);
		public BoolViewModel CreateBoolViewModel(string route) => CreateViewModel<BoolViewModel>(route);
		public FloatViewModel CreateFloatViewModel(string route) => CreateViewModel<FloatViewModel>(route);
		public StringViewModel CreateStringViewModel(string route) => CreateViewModel<StringViewModel>(route);
		public MultilineStringViewModel CreateMultiLineStringViewModel(string route) =>
			CreateViewModel<MultilineStringViewModel>(route);
		public EnumViewModel CreateEnumViewModel(string route) => CreateViewModel<EnumViewModel>(route);
		public ClassViewModel CreateClassViewModel(string route) => CreateViewModel<ClassViewModel>(route);
		public StructViewModel CreateStructViewModel(string route) => CreateViewModel<StructViewModel>(route);
		public SubtypingViewModel CreateSubtypingViewModel(string route) => CreateViewModel<SubtypingViewModel>(route);
		public BasicCollectionViewModel CreateBasicCollectionViewModel(string route) =>
			CreateViewModel<BasicCollectionViewModel>(route);
		public ComplicateCollectionViewModel CreateComplicateCollectionViewModel(string route) =>
			CreateViewModel<ComplicateCollectionViewModel>(route);
		public ReferenceByIntViewModel CreateReferenceByIntViewModel(string route) =>
			CreateViewModel<ReferenceByIntViewModel>(route);
	}
}
