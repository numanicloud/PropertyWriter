using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Livet.Messaging;
using Livet.Messaging.Windows;
using PropertyWriter.Annotation;
using PropertyWriter.Model.Info;
using PropertyWriter.ViewModel;
using Reactive.Bindings;
using PropertyWriter.Model.Properties;

namespace PropertyWriter.Model.Instance
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
