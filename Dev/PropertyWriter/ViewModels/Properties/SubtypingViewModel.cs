using System;
using Livet.Messaging;
using PropertyWriter.Models.Info;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;
using System.Reactive.Linq;
using System.Reactive;

namespace PropertyWriter.ViewModels.Properties
{
	class SubtypingViewModel : PropertyViewModel<SubtypingProperty>
	{
		private ReactiveProperty<IPropertyViewModel> Instance { get; }

        public SubTypeInfo[] AvailableTypes => Property.AvailableTypes;
		public ReactiveProperty<SubTypeInfo> SelectedType => Property.SelectedType;
		public ReactiveCommand EditCommand { get; }
		public override IObservable<Unit> OnChanged { get; }

		public SubtypingViewModel(SubtypingProperty property)
			: base(property)
        {
			Instance = Property.Model.Where(x => x != null)
				.Select(ViewModelFactory.Create)
				.ToReactiveProperty();

			EditCommand = property.Model.Select(x => x != null)
				.ToReactiveCommand();

            EditCommand.Where(x => Instance.Value != null).Subscribe(x =>
            {
				var vm = new BlockViewModel(Instance.Value);
				Messenger.Raise(new TransitionMessage(vm, "SubtypeEditor"));
            });

			OnChanged = Instance.Where(x => x != null)
				.SelectMany(x => x.OnChanged)
				.Merge(Instance.Select(x => Unit.Default));
		}
	}
}
