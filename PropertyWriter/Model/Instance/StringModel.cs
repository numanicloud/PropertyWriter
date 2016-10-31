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
	class StringModel : PropertyModel
	{
		public ReactiveProperty<string> StringValue { get; } = new ReactiveProperty<string>();
		public override ReactiveProperty<object> Value => StringValue.Select(x => (object) x)
			.ToReactiveProperty();
	}
}
