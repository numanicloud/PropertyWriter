using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet.Messaging;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
    class SubtypingModel : PropertyModel
    {
        private ModelFactory modelFactory;

        public Type BaseType { get; }
        public Type[] AvailableTypes { get; set; }
        public ReactiveProperty<Type> SelectedType { get; } = new ReactiveProperty<Type>();
        public ReactiveProperty<IPropertyModel> Model => SelectedType.Where(x => x != null)
            .Select(x => modelFactory.Create(x))
            .ToReactiveProperty();
        public override ReactiveProperty<object> Value => Model.Where(x => x != null)
            .Select(x => x.Value.Value)
            .ToReactiveProperty();

        public ReactiveCommand<PropertyModel> EditCommand { get; } = new ReactiveCommand<PropertyModel>();

        public SubtypingModel(Type baseType)
        {
            BaseType = baseType;
            EditCommand.Subscribe(x =>
            {
                Messenger.Raise(new TransitionMessage(x, "SubtypeEditor"));
            });
        }

        public SubtypingModel(Type baseType, ModelFactory modelFactory) : this(baseType)
        {
            this.modelFactory = modelFactory;
        }
    }
}
