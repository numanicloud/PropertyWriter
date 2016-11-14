using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	class StructureHolder
	{
		public IEnumerable<InstanceAndMemberInfo> Properties { get; }
		public ReactiveProperty<object> Value { get; }
		public Subject<Unit> ValueChanged { get; } = new Subject<Unit>();

		public StructureHolder(Type type, ModelFactory modelFactory)
        {
			Properties = modelFactory.LoadMembersInfo(type);

            Value = new ReactiveProperty<object> { Value = Activator.CreateInstance(type) };

            foreach (var property in Properties)
            {
                property.Model.Value.Subscribe(x =>
                {
                    property.SetValue(Value.Value, x);
					ValueChanged.OnNext(Unit.Default);
                });
            }
        }
	}
}
