using System.Reactive.Linq;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	class StringModel : PropertyModel
	{
		public ReactiveProperty<string> StringValue { get; } = new ReactiveProperty<string>();
		public override ReactiveProperty<object> Value => StringValue.Select(x => (object) x)
			.ToReactiveProperty();
	}
}
