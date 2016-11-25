using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model.Properties
{
    class BoolProperty : IPropertyModel
    {
        public ReactiveProperty<bool> BoolValue { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<object> Value { get; }

        public BoolProperty()
        {
            Value = BoolValue.Select(x => (object)x).ToReactiveProperty();
        }
    }
}
