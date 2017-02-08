using PropertyWriter.ViewModels.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Models.Properties.Interfaces;
using System.Windows.Controls;
using System.Reactive.Linq;
using System.Reactive;
using Reactive.Bindings;
using System.ComponentModel.Composition;
using PropertyWriter.ViewModels.Properties.Common;
using PropertyWriter.ViewModels.Properties.Extensibility;
using PropertyWriter.ViewModels;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Common;

namespace IntSliderPlugin
{
	//[Export(typeof(IPluginViewModelFactory))]
	public class SliderIntPlugin : IPluginViewModelFactory
	{
		public PluginViewModel CreateViewModel(IPropertyModel model, PropertyFactory propertyFactory, ViewModelFactory viewModelFactory) =>
			new SliderIntViewModel(model, viewModelFactory);
		public bool IsTargetType(Type type) => type == typeof(int);
	}

	public class SliderIntViewModel : PluginViewModel
	{
		public override UserControl UserControl => new IntSlider(this);
		public ReactiveProperty<int> IntValue { get; set; }

		public SliderIntViewModel(IPropertyModel model, ViewModelFactory factory)
			: base(model, factory)
		{
			IntValue = Compounder.CreateIntViewModel("").IntValue;
		}
	}
}
