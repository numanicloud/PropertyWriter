using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	interface IPropertyModel
	{
		ReactiveProperty<object> Value { get; }
	}
}
