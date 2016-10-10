using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using MvvmHelper;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	class StructInstance : Instance
	{
		public StructInstance( Type type )
		{
			if( !type.IsValueType )
			{
				throw new ArgumentException( "type が構造体を表す Type クラスではありません。" );
			}

			StructValue = new StructureValue( type );
		}

		public IEnumerable<IInstance> Instances => StructValue.Instances;
		public override ReactiveProperty<object> Value => StructValue.Value;

		public override ReactiveProperty<string> FormatedString
		{
			get
			{
				var events = StructValue.Properties
					.Select(x => x.Instance.Value)
					.Cast<IObservable<object>>()
					.ToArray();
				return Observable.Merge(events)
					.Select(x => Value.Value.ToString())
					.ToReactiveProperty();
			}
		}

		private StructureValue StructValue { get; set; }
	}
}
