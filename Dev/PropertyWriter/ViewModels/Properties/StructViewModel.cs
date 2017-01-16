using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Livet.Messaging;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;

namespace PropertyWriter.ViewModels.Properties
{
    internal class StructViewModel : PropertyViewModel<StructProperty>
	{
		public IPropertyViewModel[] Members { get; }
		public ReactiveCommand EditCommand { get; } = new ReactiveCommand();
		public override IObservable<Unit> OnChanged => Observable.Merge(Members.Select(x => x.OnChanged));

		public StructViewModel(StructProperty property)
			: base(property)
        {
			FormatedString = Property.OnChanged
				.Select(x => Value.Value.ToString())
				.ToReactiveProperty();

			FormatedString.Subscribe(x => { }, OnErrorSubject.OnNext);

			EditCommand.Subscribe(x => Messenger.Raise(
                new TransitionMessage(
                    new BlockViewModel(this),
                    "BlockWindow")));
			Members = property.Members.Select(ViewModelFactory.Create).ToArray();
        }
	}
}