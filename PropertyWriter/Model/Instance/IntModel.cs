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
	class IntModel : PropertyModel
	{
		public ReactiveProperty<int> IntValue { get; } = new ReactiveProperty<int>();
		public override ReactiveProperty<object> Value => IntValue.Select(x => (object)x)
			.ToReactiveProperty();
	}
}
