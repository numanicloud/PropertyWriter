using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	class ClassModel : PropertyModel
	{
		public ClassModel(Type type, ModelFactory modelFactory)
		{
			if (!type.IsClass)
			{
				throw new ArgumentException("type がクラスを表す Type クラスではありません。");
			}

			ClassValue = new StructureHolder(type, modelFactory);
		}

		private StructureHolder ClassValue { get; set; }

		public InstanceAndMemberInfo[] Members => ClassValue.Properties.ToArray();
		public override ReactiveProperty<object> Value => ClassValue.Value;

		public override ReactiveProperty<string> FormatedString
		{
			get
			{
				var events = ClassValue.Properties
					.Select(x => x.Model.Value)
					.Cast<IObservable<object>>()
					.ToArray();
				return Observable.Merge(events)
					.Select(x => Value.Value.ToString())
					.ToReactiveProperty();
			}
		}
	}
}
