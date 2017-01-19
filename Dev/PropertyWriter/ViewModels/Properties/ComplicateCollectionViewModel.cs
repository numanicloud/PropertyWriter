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
using System.Collections.Generic;

namespace PropertyWriter.ViewModels.Properties
{
	public class ComplicateCollectionViewModel : PropertyViewModel<ComplicateCollectionProperty>
	{
		private Subject<Unit> OnChangedSubject { get; } = new Subject<Unit>();

		public ReadOnlyReactiveCollection<IPropertyViewModel> Collection { get; }

		public override IObservable<Unit> OnChanged => OnChangedSubject;
		public ReactiveCommand AddCommand { get; } = new ReactiveCommand();
		public ReactiveCommand<int> RemoveCommand { get; } = new ReactiveCommand<int>();
		public ReactiveCommand EditCommand { get; } = new ReactiveCommand();

		public ComplicateCollectionViewModel(ComplicateCollectionProperty property, ViewModelFactory factory)
			: base(property)
		{
			Collection = property.Collection.ToReadOnlyReactiveCollection(x =>
			{
				var vm = factory.Create(x);
				vm.OnChanged.Subscribe(y => OnChangedSubject.OnNext(Unit.Default));
				return vm;
			});

			FormatedString = Property.Value
				.Select(x => "Count = " + Collection.Count)
				.ToReactiveProperty();

			AddCommand.Subscribe(x =>
			{
				try
				{
					Property.AddNewElement();
				}
				catch(Exception e)
				{
					OnErrorSubject.OnNext(e);
				}
				OnChangedSubject.OnNext(Unit.Default);
			});
			RemoveCommand.Subscribe(x =>
			{
				try
				{
					Property.RemoveElementAt(x);
				}
				catch (Exception e)
				{
					OnErrorSubject.OnNext(e);
				}
				OnChangedSubject.OnNext(Unit.Default);
			});
			EditCommand.Subscribe(x => Messenger.Raise(
				new TransitionMessage(
					new BlockViewModel(this),
					"BlockWindow")));
		}
	}
}