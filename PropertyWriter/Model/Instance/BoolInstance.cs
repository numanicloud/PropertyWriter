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
	class BoolInstance : Instance
	{
		public ReactiveProperty<bool> BoolValue { get; } = new ReactiveProperty<bool>();
		public override ReactiveProperty<object> Value => BoolValue.Select(x => (object) x)
			.ToReactiveProperty();
	}
}
