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
    class StringProperty : PropertyModel
    {
        public ReactiveProperty<string> StringValue { get; } = new ReactiveProperty<string>();
        public override ReactiveProperty<object> Value { get; }

        public StringProperty()
        {
            Value = StringValue.Select(x => (object)x).ToReactiveProperty();
        }
    }
}
