using System.Reflection;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties.Interfaces
{
    abstract class PropertyModel : IPropertyModel
    {
        public abstract ReactiveProperty<object> Value { get; }
        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();
        public PropertyInfo PropertyInfo { get; set; }
    }
}
