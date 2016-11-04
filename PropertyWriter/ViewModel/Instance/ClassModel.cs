using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using Livet.Messaging;
using PropertyWriter.ViewModel;
using PropertyWriter.ViewModel.Instance;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	class ClassModel : PropertyModel, IStructureModel
	{
		public Type Type { get; private set; }

		public ClassModel(Type type, ModelFactory modelFactory)
		{
			Type = type;
			if (!type.IsClass)
			{
				throw new ArgumentException("type がクラスを表す Type クラスではありません。");
			}

			ClassValue = new StructureHolder(type, modelFactory);

			Value = new ReactiveProperty<object>(ClassValue.Value.Value, ReactivePropertyMode.RaiseLatestValueOnSubscribe);
			ClassValue.ValueChanged
				.Do(x => Debugger.Log(0, "Info", $"StructureHolder {Title.Value}: {ClassValue.Value.Value}\n"))
				.Subscribe(x => Value.Value = ClassValue.Value.Value);

			EditCommand.Subscribe(x => Messenger.Raise(
				new TransitionMessage(
					new BlockViewModel(this),
					"BlockWindow")));
		}

		private StructureHolder ClassValue { get; set; }

		public InstanceAndMemberInfo[] Members => ClassValue.Properties.ToArray();
		public override ReactiveProperty<object> Value { get; }
		public ReactiveCommand<PropertyModel> EditCommand { get; } = new ReactiveCommand<PropertyModel>();

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
