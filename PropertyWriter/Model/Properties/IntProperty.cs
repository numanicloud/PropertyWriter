using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model.Properties
{
    class IntProperty
    {
        public ReactiveProperty<int> IntValue { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<object> Value { get; }

        public IntProperty()
        {
            Value = IntValue.Select(x => (object)x).ToReactiveProperty();
        }
    }
}
