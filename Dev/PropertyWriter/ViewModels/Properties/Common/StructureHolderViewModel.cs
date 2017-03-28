using PropertyWriter.Models.Properties.Common;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.ViewModels.Properties.Common
{
	public abstract class StructureHolderViewModel<TProperty> : PropertyViewModel<TProperty>
		where TProperty : IPropertyModel, IStructureProperty
	{
		public override IObservable<Unit> OnChanged { get; }
		public ReactiveProperty<IPropertyViewModel> PropertyClosedUp { get; private set; }
		public IPropertyViewModel[] Members { get; }
		public ReactiveCommand EditCommand { get; } = new ReactiveCommand();

		public StructureHolderViewModel(TProperty property, ViewModelFactory factory)
			: base(property)
		{
			PropertyClosedUp = new ReactiveProperty<IPropertyViewModel>();
			Members = property.Members.Select(x => factory.Create(x, true)).ToArray();
			OnChanged = Observable.Merge(Members.Select(x => x.OnChanged));
			EditCommand.Subscribe(x => ShowDetailSubject.OnNext(Unit.Default));
			
			foreach (var member in Members)
			{
				member.ShowDetail.Subscribe(x => PropertyClosedUp.Value = member);
			}
		}
	}
}
