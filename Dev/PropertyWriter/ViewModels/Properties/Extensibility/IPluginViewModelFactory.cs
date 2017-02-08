using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels.Properties.Common;
using PropertyWriter.ViewModels.Properties.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PropertyWriter.ViewModels.Properties
{
	public interface IPluginViewModelFactory
	{
		bool IsTargetType(Type type);
		PluginViewModel CreateViewModel(IPropertyModel model, PropertyFactory propertyFactory, ViewModelFactory viewModelFactory);
	}
}
