using System.Reactive.Linq;
using Reactive.Bindings;
using PropertyWriter.Models.Properties.Interfaces;
using System.Reactive;
using System;

namespace PropertyWriter.ViewModels.Properties.Common
{
	abstract class PropertyViewModel<TProperty> : Livet.ViewModel, IPropertyViewModel
		where TProperty : IPropertyModel
	{
		public TProperty Property { get; protected set; }
		public ReactiveProperty<string> Title => Property.Title;
		public ReactiveProperty<object> Value => Property.Value;
		public virtual ReactiveProperty<string> FormatedString => Value.Select(x => x?.ToString())
			.ToReactiveProperty();
		public abstract IObservable<Unit> OnChanged { get; }

		public PropertyViewModel(TProperty property)
		{
			Property = property;
		}

		public override string ToString() => $"<{GetType()}: {Value}>";
	}
}
