using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	abstract class Instance : IInstance
	{
		public abstract ReactiveProperty<object> Value { get; }
		public virtual ReactiveProperty<string> FormatedString => Value.Select(x => x.ToString())
			.ToReactiveProperty();
	}
}
