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

namespace PropertyWriter.Model.Instance
{
	class SubtypingModel : PropertyViewModel
	{
		public Type BaseType { get; }
		public SubTypeInfo[] AvailableTypes { get; set; }
		public ReactiveProperty<SubTypeInfo> SelectedType { get; } = new ReactiveProperty<SubTypeInfo>();
		public ReactiveProperty<IPropertyViewModel> Model { get; } = new ReactiveProperty<IPropertyViewModel>();
		public override ReactiveProperty<object> Value { get; }

		public ReactiveCommand<PropertyViewModel> EditCommand { get; } = new ReactiveCommand<PropertyViewModel>();

		public SubtypingModel(Type baseType, ModelFactory modelFactory, Type[] availableTypes)
		{
			AvailableTypes = availableTypes.Select(x =>
			{
				var attr = x.GetCustomAttribute<PwSubtypeAttribute>();
				return new SubTypeInfo(x, attr.Name);
			}).ToArray();
			BaseType = baseType;

			SelectedType.Where(x => x != null)
				.Subscribe(x => Model.Value = modelFactory.Create(x.Type, Title.Value));
			EditCommand.Subscribe(x =>
			{
				Messenger.Raise(new TransitionMessage(new BlockViewModel(Model.Value), "SubtypeEditor"));
			});

			Value = Model.Where(x => x != null)
				.SelectMany(x => x.Value)
				.Do(x => Debugger.Log(0, "Info", $"Subtype {Title.Value}: {x}\n"))
				.ToReactiveProperty(mode: ReactivePropertyMode.RaiseLatestValueOnSubscribe);
		}
	}
}
