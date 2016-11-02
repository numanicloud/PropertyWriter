using System;
using System.Collections.Generic;
using System.Linq;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	class StructureHolder
	{
        private ModelFactory modelFactory;
        
        public StructureHolder(Type type, ModelFactory modelFactory)
        {
            this.modelFactory = modelFactory;
            Properties = EntityLoader.LoadMembers(type, modelFactory);

            Value = new ReactiveProperty<object> { Value = Activator.CreateInstance(type) };

            foreach (var property in Properties)
            {
                property.Model.Value.Subscribe(x =>
                {
                    //var value = InstanceConverter.Convert(x, property.Type);
                    property.SetValue(Value.Value, x);
                });
            }
        }

        public IEnumerable<InstanceAndMemberInfo> Properties { get; }

		public ReactiveProperty<object> Value { get; }
	}
}
