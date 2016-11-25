using System;
using Livet.Messaging;
using PropertyWriter.Models.Info;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;

namespace PropertyWriter.ViewModels.Properties
{
	class SubtypingViewModel : PropertyViewModel
	{
        private SubtypingProperty Property { get; }

        public Type BaseType => Property.BaseType;
        public SubTypeInfo[] AvailableTypes => Property.AvailableTypes;
		public ReactiveProperty<SubTypeInfo> SelectedType => Property.SelectedType;
		public ReactiveProperty<IPropertyModel> Model => Property.Model;
        public override ReactiveProperty<object> Value => Property.Value;

		public ReactiveCommand<PropertyViewModel> EditCommand { get; } = new ReactiveCommand<PropertyViewModel>();

        public SubtypingViewModel(SubtypingProperty property)
        {
            Property = property;
            EditCommand.Subscribe(x =>
            {
                Messenger.Raise(new TransitionMessage(new BlockViewModel(Model.Value), "SubtypeEditor"));
            });
        }
	}
}
