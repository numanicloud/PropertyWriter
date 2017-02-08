using Livet.Messaging;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.ViewModels.Properties.Common
{
	public abstract class CollectionViewModel<TProperty> : PropertyViewModel<TProperty>
		where TProperty : ICollectionProperty
	{
		private Subject<Unit> OnChangedSubject { get; } = new Subject<Unit>();

		public ReadOnlyReactiveCollection<IPropertyViewModel> Collection { get; }
		public ReactiveProperty<int> SelectedIndex { get; } = new ReactiveProperty<int>();

		public override IObservable<Unit> OnChanged => OnChangedSubject;
		public ReactiveCommand AddCommand { get; } = new ReactiveCommand();
		public ReactiveCommand<int> RemoveCommand { get; } = new ReactiveCommand<int>();
		public ReactiveCommand EditCommand { get; } = new ReactiveCommand();
		public ReactiveCommand<int> UpCommand { get; } = new ReactiveCommand<int>();
		public ReactiveCommand<int> DownCommand { get; } = new ReactiveCommand<int>();

		public CollectionViewModel(TProperty property, ViewModelFactory factory) : base(property)
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
				catch (Exception e)
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

			UpCommand.Subscribe(x =>
			{
				Property.Move(x - 1, x);
				SelectedIndex.Value = x - 1;
			});
			DownCommand.Subscribe(x =>
			{
				Property.Move(x + 1, x);
				SelectedIndex.Value = x;
			});
		}
	}
}
