using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	class StructureHolder
	{
		public StructureHolder(Type type)
		{
			Properties = DllLoader.LoadProperties(type)
				.Select(_ => new InstanceAndPropertyInfo(_))
				.ToArray();

			Value = new ReactiveProperty<object> { Value = Activator.CreateInstance(type) };

			foreach (var property in Properties)
			{
				property.Instance.Value.Subscribe(x =>
				{
					var value = InstanceConverter.Convert(x, property.PropertyInfo.PropertyType);
					property.PropertyInfo.SetValue(Value.Value, value);
				});
			}
		}

		public IEnumerable<IPropertyModel> Instances => Properties.Select(_ => _.Instance).ToArray();

		public IEnumerable<InstanceAndPropertyInfo> Properties { get; }

		public ReactiveProperty<object> Value { get; }
	}
}
