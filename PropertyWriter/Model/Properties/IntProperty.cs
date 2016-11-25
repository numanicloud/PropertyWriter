using PropertyWriter.Model.Interfaces;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model.Properties
{
    class IntProperty : PropertyModel
    {
        public ReactiveProperty<int> IntValue { get; } = new ReactiveProperty<int>();
        public override ReactiveProperty<object> Value { get; }

        public IntProperty()
        {
            Value = IntValue.Select(x => (object)x).ToReactiveProperty();
        }
    }
}
