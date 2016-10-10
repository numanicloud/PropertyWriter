using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	internal class ComplicateCollectionInstance : Instance
	{
		public ObservableCollection<IInstance> Collection => ComplicateCollectionValue.Collection;

		public override ReactiveProperty<object> Value => ComplicateCollectionValue.Value
			.Cast<object>()
			.ToReactiveProperty();

		private CollectionValue ComplicateCollectionValue { get; }

		public ComplicateCollectionInstance(Type type)
		{
			ComplicateCollectionValue = new CollectionValue(type);
		}

		public void AddNewProperty() => ComplicateCollectionValue.AddNewProperty();
		public void RemoveAt(int index) => ComplicateCollectionValue.RemoveAt(index);
	}
}