using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	internal class ComplicateCollectionModel : PropertyModel
	{
		public ObservableCollection<IPropertyModel> Collection => ComplicateCollectionValue.Collection;
		public override ReactiveProperty<object> Value => ComplicateCollectionValue.Value
			.Cast<object>()
			.ToReactiveProperty();
		public ReactiveCommand AddCommand { get; private set; } = new ReactiveCommand();
		public ReactiveCommand<int> RemoveCommand { get; private set; } = new ReactiveCommand<int>();

		private CollectionHolder ComplicateCollectionValue { get; }

		public ComplicateCollectionModel(Type type)
		{
			ComplicateCollectionValue = new CollectionHolder(type);
			AddCommand.Subscribe(x => ComplicateCollectionValue.AddNewProperty());
			RemoveCommand.Subscribe(x => ComplicateCollectionValue.RemoveAt(x));
		}
	}
}