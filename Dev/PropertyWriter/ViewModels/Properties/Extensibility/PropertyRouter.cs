using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.ViewModels.Properties.Common;
using PropertyWriter.ViewModels.Properties;
using System.Reactive;
using System.Reactive.Linq;

namespace PropertyWriter.ViewModels
{
	public class PropertyRouter
	{
		private ViewModelFactory factory_;

		public PropertyRouter(ViewModelFactory factory)
		{
			factory_ = factory;
		}

		public ReactiveProperty<int> GetIntProperty(IPropertyModel model, string route)
		{
			var m = GetValuePropertyModel(model, route);
			if (m is IntProperty p)
			{
				return p.IntValue;
			}
			else if(m is ReferenceByIntProperty q)
			{
				return q.IntValue;
			}
			throw new InvalidCastException($"{route} は int 型ではありません。");
		}

		public ReactiveProperty<bool> GetBoolProperty(IPropertyModel model, string route)
		{
			var m = GetValuePropertyModel(model, route);
			if (m is BoolProperty p)
			{
				return p.BoolValue;
			}
			throw new InvalidCastException($"{route} は int 型ではありません。");
		}

		public ReactiveProperty<float> GetFloatProperty(IPropertyModel model, string route)
		{
			var m = GetValuePropertyModel(model, route);
			if (m is FloatProperty p)
			{
				return p.FloatValue;
			}
			throw new InvalidCastException($"{route} は int 型ではありません。");
		}

		public ReactiveProperty<string> GetStringProperty(IPropertyModel model, string route)
		{
			var m = GetValuePropertyModel(model, route);
			if (m is StringProperty p)
			{
				return p.StringValue;
			}
			throw new InvalidCastException($"{route} は int 型ではありません。");
		}

		public IPropertyViewModel CreateViewModel(IPropertyModel model, string route, bool usePlugin = true)
		{
			var m = GetValuePropertyModel(model, route);
			return factory_.Create(m, usePlugin);
		}

		public IPropertyModel GetValuePropertyModel(IPropertyModel model, string route)
		{
			return GetValueProperty(model, route.Split('.'));
		}

		private IPropertyModel GetValueProperty(IPropertyModel model, string[] symbols)
		{
			IPropertyModel GetValueOfStructure(IStructureProperty holder) => GetValueProperty(
				holder.Members.First(x => x.PropertyInfo.Name == symbols[0]),
				symbols.Skip(1).ToArray());

			if (!symbols.Any(x => x != ""))
			{
				return model;
			}

			switch (model)
			{
			case ClassProperty p: return GetValueOfStructure(p);
			case StructProperty p: return GetValueOfStructure(p);
			default: throw new ArgumentException($"ルーティングをサポートしているのは{nameof(ClassProperty)}と{nameof(StructProperty)}のみです。");
			}
		}
	}

	/// <summary>
	/// ステートフルなビュー モデル ファクトリ。
	/// </summary>
	public class PropertyCompounder
	{
		IPropertyModel model;
		PropertyRouter router;

		public bool UsePlugin { get; set; }
		public IObservable<Unit> CompoundOnChangedObservable { get; private set; }

		public PropertyCompounder(IPropertyModel model, ViewModelFactory factory)
		{
			UsePlugin = true;
			this.model = model;
			router = new PropertyRouter(factory);
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
