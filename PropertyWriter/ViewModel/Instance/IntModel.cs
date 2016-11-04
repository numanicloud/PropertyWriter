using System.Reactive.Linq;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	class IntModel : PropertyModel
	{
		public ReactiveProperty<int> IntValue { get; } = new ReactiveProperty<int>();
		public override ReactiveProperty<object> Value => IntValue.Select(x => (object)x)
			.ToReactiveProperty();
	}
}
