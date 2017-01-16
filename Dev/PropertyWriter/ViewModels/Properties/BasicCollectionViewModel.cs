﻿using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Livet.Messaging;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;
using System.Reactive;
using System.Reactive.Subjects;
using System.Diagnostics;

namespace PropertyWriter.ViewModels.Properties
{
    internal class BasicCollectionViewModel : PropertyViewModel<BasicCollectionProperty>
	{
		private Subject<Unit> OnChangedSubject { get; } = new Subject<Unit>();

        public ReadOnlyReactiveCollection<IPropertyViewModel> Collection { get; }
		public override IObservable<Unit> OnChanged => OnChangedSubject;
		public ReactiveCommand AddCommand { get; } = new ReactiveCommand();
        public ReactiveCommand<int> RemoveCommand { get; } = new ReactiveCommand<int>();
        public ReactiveCommand EditCommand { get; set; } = new ReactiveCommand();

        public BasicCollectionViewModel(BasicCollectionProperty property)
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
			}, exception => OnChangedSubject.OnError(exception));
            RemoveCommand.Subscribe(x =>
			{
				Property.RemoveAt(x);
				OnChangedSubject.OnNext(Unit.Default);
			}, exception => OnChangedSubject.OnError(exception));
			EditCommand.Subscribe(x => Messenger.Raise(
				new TransitionMessage(
					new BlockViewModel(this),
					"BlockWindow")));
		}
	}
}