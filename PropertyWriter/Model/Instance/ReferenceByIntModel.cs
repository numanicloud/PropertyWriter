using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;
using System.Diagnostics;

namespace PropertyWriter.Model.Instance
{
	class ReferenceByIntModel : PropertyModel
	{
		public Type Type { get; }
		private readonly Func<object, int> _selectId;

		public ReadOnlyReactiveCollection<object> Source { get; set; }
		public ReactiveProperty<object> SelectedObject { get; } = new ReactiveProperty<object>();
		public ReactiveProperty<int> IntValue => SelectedObject.Where(x => x != null)
            .Select(x => _selectId?.Invoke(x) ?? -1)
			.ToReactiveProperty();
		public override ReactiveProperty<object> Value => IntValue.Select(x => (object)x).ToReactiveProperty();

		public ReferenceByIntModel(Type type, string idFieldName)
		{
			Type = type;
			
			var property = type.GetProperty(idFieldName);
			if (property != null)
			{
				_selectId = obj => (int)property.GetValue(obj);
			}

			var field = type.GetField(idFieldName);
			if (field != null)
			{
				_selectId = obj => (int)field.GetValue(obj);
			}
		}
	}
}
