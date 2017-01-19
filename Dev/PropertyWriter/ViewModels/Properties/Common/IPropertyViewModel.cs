using Reactive.Bindings;
using System;
using System.Reactive;

namespace PropertyWriter.ViewModels.Properties.Common
{
	public interface IPropertyViewModel
	{
		ReactiveProperty<object> Value { get; }
		ReactiveProperty<string> FormatedString { get; }
		ReactiveProperty<string> Title { get; }
		IObservable<Unit> OnChanged { get; }
		IObservable<Exception> OnError { get; }
	}
}
