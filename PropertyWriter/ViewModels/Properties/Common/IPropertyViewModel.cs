using Reactive.Bindings;

namespace PropertyWriter.ViewModels.Properties.Common
{
	interface IPropertyViewModel
	{
		ReactiveProperty<object> Value { get; }
		ReactiveProperty<string> FormatedString { get; }
		ReactiveProperty<string> Title { get; set; }
	}
}
