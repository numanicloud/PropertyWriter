using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Livet.Messaging;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;

namespace PropertyWriter.ViewModels.Properties
{
    internal class BasicCollectionViewModel : PropertyViewModel
	{
        private BasicCollectionProperty Property { get; }

        public ObservableCollection<IPropertyModel> Collection => Property.Collection;
        public override ReactiveProperty<object> Value => Property.Value;

        public override ReactiveProperty<string> FormatedString { get; }

        public ReactiveCommand AddCommand { get; } = new ReactiveCommand();
        public ReactiveCommand<int> RemoveCommand { get; } = new ReactiveCommand<int>();
        public ReactiveCommand EditCommand { get; set; } = new ReactiveCommand();

        public BasicCollectionViewModel(BasicCollectionProperty property)
        {
            Property = property;

            FormatedString = Property.Value
                .Select(x => "Count = " + Collection.Count)
                .ToReactiveProperty();

            EditCommand.Subscribe(x => Messenger.Raise(
                new TransitionMessage(
                    new BlockViewModel(Property),
                    "BlockWindow")));
            AddCommand.Subscribe(x => Property.AddNewElement());
            RemoveCommand.Subscribe(x => Property.RemoveAt(x));
        }
	}
}