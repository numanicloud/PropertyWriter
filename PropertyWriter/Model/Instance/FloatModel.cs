using System.Reactive.Linq;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	class FloatModel : PropertyModel
	{
		public ReactiveProperty<float> FloatValue { get; } = new ReactiveProperty<float>();
		public override ReactiveProperty<object> Value => FloatValue.Select(x => (object) x)
			.ToReactiveProperty();
	}
}
