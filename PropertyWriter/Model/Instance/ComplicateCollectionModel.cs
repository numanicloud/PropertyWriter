using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	internal class ComplicateCollectionModel : PropertyModel
	{
        private ModelFactory modelFactory;

        public ObservableCollection<IPropertyModel> Collection => ComplicateCollectionValue.Collection;
		public override ReactiveProperty<object> Value => ComplicateCollectionValue.Value
			.Cast<object>()
			.ToReactiveProperty();
		public ReactiveCommand AddCommand { get; private set; } = new ReactiveCommand();
		public ReactiveCommand<int> RemoveCommand { get; private set; } = new ReactiveCommand<int>();
		public Type ElementType => ComplicateCollectionValue.ItemType;

		private CollectionHolder ComplicateCollectionValue { get; }

        public ComplicateCollectionModel(Type type, ModelFactory modelFactory)
        {
            this.modelFactory = modelFactory;
			ComplicateCollectionValue = new CollectionHolder(type, modelFactory);
			AddCommand.Subscribe(x => ComplicateCollectionValue.AddNewProperty());
			RemoveCommand.Subscribe(x => ComplicateCollectionValue.RemoveAt(x));
		}
    }
}