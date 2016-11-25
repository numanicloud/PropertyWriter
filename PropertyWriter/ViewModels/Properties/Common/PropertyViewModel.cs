using System.Reactive.Linq;
using Reactive.Bindings;

namespace PropertyWriter.ViewModels.Properties.Common
{
	abstract class PropertyViewModel : Livet.ViewModel, IPropertyViewModel
	{
		public ReactiveProperty<string> Title { get; set; } = new ReactiveProperty<string>();
		public abstract ReactiveProperty<object> Value { get; }
		public virtual ReactiveProperty<string> FormatedString => Value.Select(x => x?.ToString())
			.ToReactiveProperty();

		public override string ToString() => $"<{GetType()}: {Value}>";
	}
}
