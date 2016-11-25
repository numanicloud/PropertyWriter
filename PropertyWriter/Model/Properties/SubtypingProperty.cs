using PropertyWriter.Annotation;
using PropertyWriter.Model.Info;
using PropertyWriter.Model.Instance;
using PropertyWriter.Model.Interfaces;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model.Properties
{
    class SubtypingProperty : PropertyModel
    {
        public Type BaseType { get; }
        public SubTypeInfo[] AvailableTypes { get; set; }
        public ReactiveProperty<SubTypeInfo> SelectedType { get; } = new ReactiveProperty<SubTypeInfo>();
        public ReactiveProperty<IPropertyModel> Model { get; } = new ReactiveProperty<IPropertyModel>();
        public override ReactiveProperty<object> Value { get; }

        public SubtypingProperty(Type baseType, PropertyFactory modelFactory, Type[] availableTypes)
        {
            AvailableTypes = availableTypes.Select(x =>
            {
                var attr = x.GetCustomAttribute<PwSubtypeAttribute>();
                return new SubTypeInfo(x, attr.Name);
            }).ToArray();
            BaseType = baseType;

            SelectedType.Where(x => x != null)
                .Subscribe(x => Model.Value = modelFactory.Create(x.Type, Title.Value));

            Value = Model.Where(x => x != null)
                .SelectMany(x => x.Value)
                .ToReactiveProperty(mode: ReactivePropertyMode.RaiseLatestValueOnSubscribe);
        }
    }
}
