using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Livet.Messaging;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;
using System.Diagnostics;

namespace PropertyWriter.ViewModels.Properties
{
    class ClassViewModel : PropertyViewModel<ClassProperty>
	{
        public IPropertyViewModel[] Members { get; }
        public ReactiveCommand EditCommand { get; } = new ReactiveCommand();
		public override IObservable<Unit> OnChanged => Observable.Merge(Members.Select(x => x.OnChanged));

        public ClassViewModel(ClassProperty property)
			: base(property)
        {
			FormatedString.Dispose();
			FormatedString = Property.OnChanged
				.Select(x => Value.Value.ToString())
				.Do(x => { }, ex => Debugger.Log(1, "Error", $"Error from ClassViewModel:\n{ex}\n"))
				.ToReactiveProperty(Value.Value.ToString());

			EditCommand.Subscribe(x => Messenger.Raise(
                new TransitionMessage(
                    new BlockViewModel(this),
                    "BlockWindow")));
			Members = property.Members.Select(ViewModelFactory.Create).ToArray();
        }
	}
}
