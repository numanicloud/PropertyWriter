using PropertyWriter.ViewModels.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.Models.Properties;
using System.Reactive;
using System.Windows.Controls;
using System.Reactive.Linq;
using PropertyWriter.ViewModels.Properties.Common;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace IntSliderPlugin
{
	[Export(typeof(IPluginViewModelFactory))]
	public class ToleranceViewModelFactory : IPluginViewModelFactory
	{
		public Type EntityType => typeof(RpgData.Tolerance);
		public PluginViewModel CreateViewModel(IPropertyModel model, ViewModelFactory factory) =>
			new ToleranceViewModel(model, factory);
	}

	public class ToleranceViewModel : PluginViewModel
	{
		public IntViewModel Blow { get; }
		public IntViewModel Gash { get; }
		public IntViewModel Burn { get; }
		public IntViewModel Chill { get; }
		public IntViewModel Electric { get; }
		public IntViewModel Primal { get; }

		[Import("ToleranceView")]
		public override UserControl UserControl { get; }
		public override IObservable<Unit> OnChanged { get; }

		public ToleranceViewModel(IPropertyModel model, ViewModelFactory factory) : base(model, factory)
		{
			IntViewModel Create(string key) => (IntViewModel)Router.CreateViewModel(model, key, false);

			Blow = Create("Blow");
			Gash = Create("Gash");
			Burn = Create("Burn");
			Chill = Create("Chill");
			Electric = Create("Electric");
			Primal = Create("Primal");

			OnChanged = Observable.Merge(Blow.OnChanged, Gash.OnChanged, Burn.OnChanged,
				Chill.OnChanged, Electric.OnChanged, Primal.OnChanged);

			var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
			var container = new CompositionContainer(catalog);
			UserControl = container.GetExportedValue<UserControl>("ToleranceView");
			UserControl.DataContext = this;
		}
	}
}
