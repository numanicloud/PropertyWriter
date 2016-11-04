using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using Livet.Messaging;
using PropertyWriter.ViewModel;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	internal class BasicCollectionModel : PropertyModel
	{
		public ObservableCollection<IPropertyModel> Collection => CollectionValue.Collection;

		public override ReactiveProperty<object> Value => CollectionValue.Value
			.Cast<object>()
			.ToReactiveProperty();

		public override ReactiveProperty<string> FormatedString => CollectionValue.Value
			.Select(x => "Count = " + Collection.Count)
			.ToReactiveProperty();

		public ReactiveCommand AddCommand { get; } = new ReactiveCommand();
		public ReactiveCommand<int> RemoveCommand { get; } = new ReactiveCommand<int>();
		public ReactiveCommand EditCommand { get; set; } = new ReactiveCommand();

		private CollectionHolder CollectionValue { get; }

		public BasicCollectionModel(Type type, ModelFactory modelFactory)
		{
			CollectionValue = new CollectionHolder(type, modelFactory);
			EditCommand.Subscribe(x => Messenger.Raise(
				new TransitionMessage(
					new BlockViewModel(this),
					"BlockWindow")));
			AddCommand.Subscribe(x => CollectionValue.AddNewProperty());
			RemoveCommand.Subscribe(x => CollectionValue.RemoveAt(x));
		}


		public void AddNewProperty()
		{
			CollectionValue.AddNewProperty();
		}

		public void RemoveAt(int index)
		{
			CollectionValue.RemoveAt(index);
		}
	}
}