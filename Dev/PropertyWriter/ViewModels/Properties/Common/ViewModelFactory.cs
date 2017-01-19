using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace PropertyWriter.ViewModels.Properties.Common
{
	public class ViewModelFactory
	{
		public Dictionary<Type, IPluginViewModelFactory> Factories { get; set; }

		public ViewModelFactory(bool usePlugin = true)
		{
			if (!usePlugin)
			{
				Factories = new Dictionary<Type, IPluginViewModelFactory>();
				return;
			}

			if (!Directory.Exists(App.PluginDirectory))
			{
				Directory.CreateDirectory(App.PluginDirectory);
			}

			var catalog = new DirectoryCatalog(App.PluginDirectory);
			var container = new CompositionContainer(catalog);
			var plugins = container.GetExportedValues<IPluginViewModelFactory>();
			Factories = plugins.ToDictionary(x => x.EntityType);
		}

		public IPropertyViewModel Create(IPropertyModel model, bool usePlugin = true)
		{
			if (usePlugin && Factories.ContainsKey(model.ValueType))
			{
				return Factories[model.PropertyInfo.PropertyType].CreateViewModel(model, this);
			}

			switch (model)
			{
			case IntProperty p: return new IntViewModel(p);
			case BoolProperty p: return new BoolViewModel(p);
			case FloatProperty p: return new FloatViewModel(p);
			case StringProperty p when !p.IsMultiLine: return new StringViewModel(p);
			case StringProperty p when p.IsMultiLine: return new MultilineStringViewModel(p);
			case EnumProperty p: return new EnumViewModel(p);
			case ClassProperty p: return new ClassViewModel(p, this);
			case StructProperty p: return new StructViewModel(p, this);
			case SubtypingProperty p: return new SubtypingViewModel(p, this);
			case BasicCollectionProperty p: return new BasicCollectionViewModel(p, this);
			case ComplicateCollectionProperty p: return new ComplicateCollectionViewModel(p, this);
			case ReferenceByIntProperty p: return new ReferenceByIntViewModel(p);
			default: throw new ArgumentException("開発者向け：ViewModelの生成に失敗しました。");
			}
		}
	}
}
