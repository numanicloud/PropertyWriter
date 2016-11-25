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
    class BoolProperty : PropertyModel
    {
        public ReactiveProperty<bool> BoolValue { get; } = new ReactiveProperty<bool>();
        public override ReactiveProperty<object> Value { get; }

        public BoolProperty()
        {
            Value = BoolValue.Select(x => (object)x).ToReactiveProperty();
        }
    }
}
