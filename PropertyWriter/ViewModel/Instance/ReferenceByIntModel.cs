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

		public ReferencableMasterInfo Source { get; set; }
		public ReactiveProperty<object> SelectedObject { get; } = new ReactiveProperty<object>();
		public ReactiveProperty<int> IntValue { get; }
		public override ReactiveProperty<object> Value => IntValue.Select(x => (object)x).ToReactiveProperty();

		public ReferenceByIntModel(ReferencableMasterInfo source, string idFieldName)
		{
			Source = source;

			var property = Source.Type.GetProperty(idFieldName);
			if (property != null)
			{
				_selectId = obj => (int)property.GetValue(obj);
			}

			IntValue = SelectedObject.Where(x => x != null)
				.Select(x => _selectId?.Invoke(x) ?? -1)
				.ToReactiveProperty();
		}

		public void SetItemById(int id)
		{
			var obj = Source.Collection.FirstOrDefault(x => _selectId(x) == id);
			if (obj != null)
			{
				SelectedObject.Value = obj;
			}
		}
	}
}
