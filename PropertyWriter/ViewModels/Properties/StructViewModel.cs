using System;
using System.Linq;
using System.Reactive.Linq;
using Livet.Messaging;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;

namespace PropertyWriter.ViewModels.Properties
{
    internal class StructViewModel : PropertyViewModel
	{
        private StructProperty Property { get; }

        public Type Type => Property.Type;
		public IPropertyModel[] Members => Property.Members;
        public override ReactiveProperty<object> Value => Property.Value;
        public ReactiveCommand EditCommand { get; } = new ReactiveCommand();

        public override ReactiveProperty<string> FormatedString
		{
			get
			{
				var events = Members.Select(x => x.Value)
					.Cast<IObservable<object>>()
					.ToArray();
				return Observable.Merge(events)
					.Select(x => Value.Value.ToString())
					.ToReactiveProperty();
			}
		}

        public StructViewModel(StructProperty property)
        {
            Property = property;
            EditCommand.Subscribe(x => Messenger.Raise(
                new TransitionMessage(
                    new BlockViewModel(Property),
                    "BlockWindow")));
        }
	}
}