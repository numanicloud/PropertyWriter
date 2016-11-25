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
	internal class BasicCollectionViewModel : PropertyViewModel
	{
        private BasicCollectionProperty Property { get; }

        public BasicCollectionViewModel(BasicCollectionProperty property)
        {
            Property = property;
            EditCommand.Subscribe(x => Messenger.Raise(
                new TransitionMessage(
                    new BlockViewModel(this),
                    "BlockWindow")));
            AddCommand.Subscribe(x => Property.AddNewElement());
            RemoveCommand.Subscribe(x => Property.RemoveAt(x));
        }

		public ObservableCollection<IPropertyViewModel> Collection => Property.Collection;
		public override ReactiveProperty<object> Value => Property.Value;

		public override ReactiveProperty<string> FormatedString => Property.Value
			.Select(x => "Count = " + Collection.Count)
			.ToReactiveProperty();

		public ReactiveCommand AddCommand { get; } = new ReactiveCommand();
		public ReactiveCommand<int> RemoveCommand { get; } = new ReactiveCommand<int>();
		public ReactiveCommand EditCommand { get; set; } = new ReactiveCommand();
	}
}