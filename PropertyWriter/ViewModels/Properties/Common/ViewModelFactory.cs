using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.ViewModels.Properties.Common
{
	class ViewModelFactory
	{
		public static IPropertyViewModel Create(IPropertyModel model)
		{
			switch (model)
			{
			case IntProperty p: return new IntViewModel(p);
			case BoolProperty p: return new BoolViewModel(p);
			case FloatProperty p: return new FloatViewModel(p);
			case StringProperty p: return new StringViewModel(p);
			case EnumProperty p: return new EnumViewModel(p);
			case ClassProperty p: return new ClassViewModel(p);
			case StructProperty p: return new StructViewModel(p);
			case SubtypingProperty p: return new SubtypingViewModel(p);
			case BasicCollectionProperty p: return new BasicCollectionViewModel(p);
			case ComplicateCollectionProperty p: return new ComplicateCollectionViewModel(p);
			case ReferenceByIntProperty p: return new ReferenceByIntViewModel(p);
			default: throw new ArgumentException("開発者向け：ViewModelの生成に失敗しました。");
			}
		}
	}
}
