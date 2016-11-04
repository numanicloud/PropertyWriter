using System.Reactive.Linq;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	class BoolModel : PropertyModel
	{
		public ReactiveProperty<bool> BoolValue { get; } = new ReactiveProperty<bool>();
		public override ReactiveProperty<object> Value => BoolValue.Select(x => (object) x)
			.ToReactiveProperty();
	}
}
