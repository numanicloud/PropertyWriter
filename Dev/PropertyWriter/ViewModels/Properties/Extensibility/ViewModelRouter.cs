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
	public class ViewModelRouter : Models.PropertyRouter
	{
		private ViewModelFactory factory_;

		public ViewModelRouter(ViewModelFactory factory)
		{
			factory_ = factory;
		}
		public IPropertyViewModel CreateViewModel(IPropertyModel model, string route, bool usePlugin = true)
		{
			var m = GetValuePropertyModel(model, route);
			return factory_.Create(m, usePlugin);
		}
	}
}
