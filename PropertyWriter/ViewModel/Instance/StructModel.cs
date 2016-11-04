using System;
using System.Linq;
using System.Reactive.Linq;
using Livet.Messaging;
using PropertyWriter.ViewModel;
using PropertyWriter.ViewModel.Instance;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	internal class StructModel : PropertyModel, IStructureModel
	{
		public Type Type { get; private set; }
		public InstanceAndMemberInfo[] Members => StructValue.Properties.ToArray();
		public override ReactiveProperty<object> Value { get; }

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

		private StructureHolder StructValue { get; }
		public ReactiveCommand EditCommand { get; } = new ReactiveCommand();

		public StructModel(Type type, ModelFactory modelFactory)
		{
			Type = type;
			if (!type.IsValueType)
			{
				throw new ArgumentException("type が構造体を表す Type クラスではありません。");
			}

			StructValue = new StructureHolder(type, modelFactory);
			EditCommand.Subscribe(x => Messenger.Raise(
				new TransitionMessage(
					new BlockViewModel(this),
					"BlockWindow")));

			Value = StructValue.ValueChanged
				.Select(x => StructValue.Value.Value)
				.ToReactiveProperty();
		}
	}
}