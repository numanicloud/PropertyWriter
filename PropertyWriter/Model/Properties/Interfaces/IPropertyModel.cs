using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model.Properties
{
    interface IPropertyModel
    {
        ReactiveProperty<object> Value { get; }
        ReactiveProperty<string> Title { get; }
        PropertyInfo PropertyInfo { get; set; }
    }
}
