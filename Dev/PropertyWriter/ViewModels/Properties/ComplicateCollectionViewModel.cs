using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using Livet.Messaging;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;
using System.Reactive.Subjects;

namespace PropertyWriter.ViewModels.Properties
{
    internal class ComplicateCollectionViewModel : PropertyViewModel<ComplicateCollectionProperty>
	{
		private Subject<Unit> OnChangedSubject { get; } = new Subject<Unit>();

		public ReadOnlyReactiveCollection<IPropertyViewModel> Collection { get; }

        public override ReactiveProperty<string> FormatedString { get; }
		public override IObservable<Unit> OnChanged => OnChangedSubject;
		public ReactiveCommand AddCommand { get; } = new ReactiveCommand();
		public ReactiveCommand<int> RemoveCommand { get; } = new ReactiveCommand<int>();
		public ReactiveCommand EditCommand { get; } = new ReactiveCommand();

        public ComplicateCollectionViewModel(ComplicateCollectionProperty property)
			: base(property)
		{
			Collection = property.Collection.ToReadOnlyReactiveCollection(x =>
			{
				var vm = ViewModelFactory.Create(x);
				vm.OnChanged.Subscribe(y => OnChangedSubject.OnNext(Unit.Default));
				return vm;
			});

			FormatedString = Property.Value
                .Select(x => "Count = " + Collection.Count)
                .ToReactiveProperty();

            AddCommand.Subscribe(x =>
			{
				Property.AddNewElement();
				OnChangedSubject.OnNext(Unit.Default);
			});
            RemoveCommand.Subscribe(x =>
			{
				Property.RemoveElementAt(x);
				OnChangedSubject.OnNext(Unit.Default);
			});
            EditCommand.Subscribe(x => Messenger.Raise(
                new TransitionMessage(
                    new BlockViewModel(this),
                    "BlockWindow")));
        }
	}
}