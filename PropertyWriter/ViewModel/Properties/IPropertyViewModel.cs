using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	interface IPropertyViewModel
	{
		ReactiveProperty<object> Value { get; }
		ReactiveProperty<string> FormatedString { get; }
		ReactiveProperty<string> Title { get; set; }
	}
}
