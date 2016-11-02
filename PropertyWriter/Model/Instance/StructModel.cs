using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	class StructModel : PropertyModel
	{
		private ModelFactory modelFactory;

		public StructModel(Type type, ModelFactory modelFactory)
		{
			if (!type.IsValueType)
			{
				throw new ArgumentException("type が構造体を表す Type クラスではありません。");
			}

			this.modelFactory = modelFactory;
			StructValue = new StructureHolder(type, modelFactory);
		}

		public InstanceAndMemberInfo[] Members => StructValue.Properties.ToArray();
		public override ReactiveProperty<object> Value => StructValue.Value;

		public override ReactiveProperty<string> FormatedString
		{
			get
			{
				var events = StructValue.Properties
					.Select(x => x.Model.Value)
					.Cast<IObservable<object>>()
					.ToArray();
				return Observable.Merge(events)
					.Select(x => Value.Value.ToString())
					.ToReactiveProperty();
			}
		}

		private StructureHolder StructValue { get; set; }
	}
}
