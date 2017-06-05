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

namespace PropertyWriter.Models
{
	public class PropertyRouter
	{
		public PropertyRouter()
		{
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
}
