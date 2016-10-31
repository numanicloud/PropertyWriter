using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using MvvmHelper;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	class FloatModel : PropertyModel
	{
		public ReactiveProperty<float> FloatValue { get; } = new ReactiveProperty<float>();
		public override ReactiveProperty<object> Value => FloatValue.Select(x => (object) x)
			.ToReactiveProperty();
	}
}
