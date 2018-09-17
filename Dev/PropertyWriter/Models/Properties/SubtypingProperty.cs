using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using PropertyWriter.Annotation;
using PropertyWriter.Models.Info;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;
using System.Diagnostics;

namespace PropertyWriter.Models.Properties
{
    public class SubtypingProperty : PropertyModel
    {
        public override Type ValueType { get; }
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
            ValueType = baseType;
			
			SelectedType.Where(x => x != null)
				.Subscribe(x =>
				{
					try
					{
						Model.Value = modelFactory.Create(x.Type, Title.Value);
					}
					catch (Exception e)
					{
						Debugger.Log(1, "Error", $"Error from SubtypingProperty:\n{e}\n");
						OnErrorSubject.OnNext(e);
					}
				});

            Value = Model.Where(x => x != null)
                .SelectMany(x => x.Value)
                .ToReactiveProperty(mode: ReactivePropertyMode.RaiseLatestValueOnSubscribe);

			Model.Where(x => x != null)
				.SelectMany(x => x.OnError)
				.Subscribe(x => OnErrorSubject.OnNext(x));
        }

		public override void CopyFrom(IPropertyModel property)
		{
			if (property is SubtypingProperty subtypingProperty)
			{
				// 具象型が未設定の時はコピー先も未設定にする
				if (subtypingProperty.SelectedType.Value != null)
				{
					SelectedType.Value = AvailableTypes.FirstOrDefault(x => x.Type == subtypingProperty.SelectedType.Value.Type);
					Model.Value.CopyFrom(subtypingProperty.Model.Value);
				}
			}
		}
	}
}
