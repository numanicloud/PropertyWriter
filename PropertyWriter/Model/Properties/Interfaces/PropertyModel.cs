using PropertyWriter.Model.Properties;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model.Interfaces
{
    abstract class PropertyModel : IPropertyModel
    {
        public abstract ReactiveProperty<object> Value { get; }
        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();
        public PropertyInfo PropertyInfo { get; set; }
    }
}
