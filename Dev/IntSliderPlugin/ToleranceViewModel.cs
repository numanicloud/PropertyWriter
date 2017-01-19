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
using PropertyWriter.ViewModels.Properties.Extensibility;
using PropertyWriter.ViewModels;
using System.Diagnostics;

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
		[Import("ToleranceView")]
		public override UserControl UserControl { get; }

		public IntViewModel Blow { get; }
		public IntViewModel Gash { get; }
		public IntViewModel Burn { get; }
		public IntViewModel Chill { get; }
		public IntViewModel Electric { get; }
		public IntViewModel Primal { get; }

		public ToleranceViewModel(IPropertyModel model, ViewModelFactory factory)
			: base(model, factory)
		{
			var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
			var container = new CompositionContainer(catalog);
			UserControl = container.GetExportedValue<UserControl>("ToleranceView");
			UserControl.DataContext = this;

			Blow = Compounder.CreateIntViewModel("Blow");
			Gash = Compounder.CreateIntViewModel("Gash");
			Burn = Compounder.CreateIntViewModel("Burn");
			Chill = Compounder.CreateIntViewModel("Chill");
			Electric = Compounder.CreateIntViewModel("Electric");
			Primal = Compounder.CreateIntViewModel("Primal");
		}
	}
}
