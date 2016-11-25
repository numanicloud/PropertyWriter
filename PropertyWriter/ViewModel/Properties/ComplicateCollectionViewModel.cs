using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using Livet.Messaging;
using PropertyWriter.ViewModel;
using PropertyWriter.ViewModel.Instance;
using Reactive.Bindings;
using PropertyWriter.Model.Properties;

namespace PropertyWriter.Model.Instance
{
	internal class ComplicateCollectionViewModel : PropertyViewModel
	{
        private ComplicateCollectionProperty Property { get; }

		public ObservableCollection<IPropertyViewModel> Collection => Property.Collection;
        public override ReactiveProperty<object> Value => Property.Value;
        public Type ElementType => Property.ElementType;

        public override ReactiveProperty<string> FormatedString { get; }
		public ReactiveCommand AddCommand { get; } = new ReactiveCommand();
		public ReactiveCommand<int> RemoveCommand { get; } = new ReactiveCommand<int>();
		public ReactiveCommand<PropertyViewModel> EditCommand { get; } = new ReactiveCommand<PropertyViewModel>();

		private CollectionHolder ComplicateCollectionValue { get; }

        public ComplicateCollectionViewModel(ComplicateCollectionProperty property)
        {
            Property = property;

            FormatedString = ComplicateCollectionValue.Value
                .Select(x => "Count = " + Collection.Count)
                .ToReactiveProperty();

            AddCommand.Subscribe(x => ComplicateCollectionValue.AddNewElement());
            RemoveCommand.Subscribe(x => ComplicateCollectionValue.RemoveAt(x));
            EditCommand.Subscribe(x => Messenger.Raise(
                new TransitionMessage(
                    new BlockViewModel(this),
                    "BlockWindow")));
        }
	}
}