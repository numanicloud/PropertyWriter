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
using PropertyWriter.Models.Properties.Common;

namespace PropertyWriter.ViewModels.Properties.Common
{
	public class ViewModelFactory
	{
		private PropertyFactory modelFactory_;

		public List<IPluginViewModelFactory> Factories { get; set; }

		public ViewModelFactory(PropertyFactory modelFactory, bool usePlugin = true)
		{
			modelFactory_ = modelFactory;
			Factories = new List<IPluginViewModelFactory>();

			if (usePlugin)
			{
				if (!Directory.Exists(App.PluginDirectory))
				{
					Directory.CreateDirectory(App.PluginDirectory);
				}

				var catalog = new DirectoryCatalog(App.PluginDirectory);
				var container = new CompositionContainer(catalog);
				Factories = container.GetExportedValues<IPluginViewModelFactory>().ToList();
			}
		}

		public IPropertyViewModel Create(IPropertyModel model, bool usePlugin = true)
		{
			if(usePlugin)
			{
				var factory = Factories.FirstOrDefault(x => x.IsTargetType(model.ValueType));
				if (factory != null)
				{
					return factory.CreateViewModel(model, modelFactory_, this);
				}
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
			case ReferenceByIntCollectionProperty p: return new ReferenceByIntCollectionViewModel(p, this);
			case ReferenceByIntProperty p: return new ReferenceByIntViewModel(p);
			default: throw new ArgumentException("開発者向け：ViewModelの生成に失敗しました。");
			}
		}
	}
}
