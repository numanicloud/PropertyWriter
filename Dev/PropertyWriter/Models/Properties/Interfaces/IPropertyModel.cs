using System.Reflection;
using Reactive.Bindings;
using System;

namespace PropertyWriter.Models.Properties.Interfaces
{
    interface IPropertyModel
    {
        ReactiveProperty<object> Value { get; }
        ReactiveProperty<string> Title { get; }
        PropertyInfo PropertyInfo { get; set; }
		IObservable<Exception> OnError { get; }
    }
}
