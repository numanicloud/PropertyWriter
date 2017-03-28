using System;
using Livet.Messaging;
using PropertyWriter.Models.Info;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;
using System.Reactive.Linq;
using System.Reactive;

namespace PropertyWriter.ViewModels.Properties
{
	public class SubtypingViewModel : PropertyViewModel<SubtypingProperty>
	{
		public ReactiveProperty<IPropertyViewModel> Instance { get; }

        public SubTypeInfo[] AvailableTypes => Property.AvailableTypes;
		public ReactiveProperty<SubTypeInfo> SelectedType => Property.SelectedType;
		public ReactiveCommand EditCommand { get; }
		public override IObservable<Unit> OnChanged { get; }

		public SubtypingViewModel(SubtypingProperty property, ViewModelFactory factory)
			: base(property)
        {
			Instance = Property.Model.Where(x => x != null)
				.Select(x => factory.Create(x, true))
				.ToReactiveProperty();

			EditCommand = property.Model.Select(x => x != null)
				.ToReactiveCommand();

            EditCommand.Where(x => Instance.Value != null).Subscribe(x =>
            {
				ShowDetailSubject.OnNext(Instance.Value);
            });
			Instance.Subscribe(x => ShowDetailSubject.OnNext(null));

			OnChanged = Instance.Where(x => x != null)
				.SelectMany(x => x.OnChanged)
				.Merge(Instance.Select(x => Unit.Default));
		}
	}
}
