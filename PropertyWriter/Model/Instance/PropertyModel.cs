using System.Reactive.Linq;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	abstract class PropertyModel : IPropertyModel
	{
		public abstract ReactiveProperty<object> Value { get; }
		public virtual ReactiveProperty<string> FormatedString => Value.Select(x => x.ToString())
			.ToReactiveProperty();
	}
}
