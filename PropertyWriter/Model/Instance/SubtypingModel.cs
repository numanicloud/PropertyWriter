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
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
    class SubtypingModel : PropertyModel
    {
	    public Type BaseType { get; }
        public SubTypeInfo[] AvailableTypes { get; set; }
        public ReactiveProperty<SubTypeInfo> SelectedType { get; } = new ReactiveProperty<SubTypeInfo>();
        public ReactiveProperty<IPropertyModel> Model { get; } = new ReactiveProperty<IPropertyModel>();
        public override ReactiveProperty<object> Value => Model.Where(x => x != null)
            .Select(x => x.Value.Value)
            .ToReactiveProperty();

        public ReactiveCommand<PropertyModel> EditCommand { get; } = new ReactiveCommand<PropertyModel>();

        public SubtypingModel(Type baseType, ModelFactory modelFactory, Type[] availableTypes)
        {
	        AvailableTypes = availableTypes.Select(x =>
	        {
		        var attr = x.GetCustomAttribute<PwSubtypeAttribute>();
				return new SubTypeInfo(x, attr.Name);
	        }).ToArray();
	        BaseType = baseType;

	        SelectedType.Do(x => Debugger.Log(0, "Info", $"SelectedType: {x}\n"))
				.Where(x => x != null)
				.Subscribe(x => Model.Value = modelFactory.Create(x.Type));
			EditCommand.Subscribe(x =>
			{
				Messenger.Raise(new TransitionMessage(x, "SubtypeEditor"));
			});
		}
    }
}
