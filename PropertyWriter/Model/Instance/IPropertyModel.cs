using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	interface IPropertyModel
	{
		ReactiveProperty<object> Value { get; }
		ReactiveProperty<string> FormatedString { get; }
	}
}
