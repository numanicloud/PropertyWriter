using System.Reflection;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties.Interfaces
{
    interface IPropertyModel
    {
        ReactiveProperty<object> Value { get; }
        ReactiveProperty<string> Title { get; }
        PropertyInfo PropertyInfo { get; set; }
    }
}
