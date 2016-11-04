using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using Livet.Messaging;
using PropertyWriter.ViewModel;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	internal class ComplicateCollectionModel : PropertyModel
	{
		public ObservableCollection<IPropertyModel> Collection => ComplicateCollectionValue.Collection;
		public override ReactiveProperty<object> Value { get; }
		public override ReactiveProperty<string> FormatedString { get; }
		public ReactiveCommand AddCommand { get; } = new ReactiveCommand();
		public ReactiveCommand<int> RemoveCommand { get; } = new ReactiveCommand<int>();
		public ReactiveCommand<PropertyModel> EditCommand { get; } = new ReactiveCommand<PropertyModel>();
		public Type ElementType => ComplicateCollectionValue.ItemType;

		private CollectionHolder ComplicateCollectionValue { get; }

        public ComplicateCollectionModel(Type type, ModelFactory modelFactory)
        {
	        ComplicateCollectionValue = new CollectionHolder(type, modelFactory);
	        Value = ComplicateCollectionValue.Value
		        .Cast<object>()
		        .ToReactiveProperty();
	        FormatedString = ComplicateCollectionValue.Value
		        .Select(x => "Count = " + Collection.Count)
		        .ToReactiveProperty();

			AddCommand.Subscribe(x => ComplicateCollectionValue.AddNewProperty());
			RemoveCommand.Subscribe(x => ComplicateCollectionValue.RemoveAt(x));
			EditCommand.Subscribe(x => Messenger.Raise(
				new TransitionMessage(
					new BlockViewModel(this),
					"BlockWindow")));
		}
    }
}