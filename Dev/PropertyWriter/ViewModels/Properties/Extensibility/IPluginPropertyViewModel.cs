using PropertyWriter.ViewModels.Properties.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;
using System.Reactive;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.Models.Properties.Common;
using System.Windows.Controls;
using PropertyWriter.Models.Properties;
using System.Reactive.Linq;
using System.Collections.Specialized;
using PropertyWriter.ViewModels.Properties.Extensibility;

namespace PropertyWriter.ViewModels.Properties
{
	public abstract class PluginViewModel : PropertyViewModel<IPropertyModel>, IPluginViewModel
	{
		private IObservable<Unit> onChanged_;

		protected PropertyCompounder Compounder { get; }
		public abstract UserControl UserControl { get; }
		public override IObservable<Unit> OnChanged => onChanged_ ?? (onChanged_ = Compounder.CompoundOnChangedObservable);

		public PluginViewModel(IPropertyModel model, ViewModelFactory factory)
			: base(model)
		{
			Compounder = new PropertyCompounder(model, factory)
			{
				UsePlugin = false,
			};
		}
	}
}
