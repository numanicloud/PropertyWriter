using System.Reactive.Linq;
using Reactive.Bindings;
using PropertyWriter.Models.Properties.Interfaces;
using System.Reactive;
using System;
using System.Diagnostics;
using System.Reactive.Subjects;

namespace PropertyWriter.ViewModels.Properties.Common
{
	public abstract class PropertyViewModel<TProperty> : Livet.ViewModel, IPropertyViewModel
		where TProperty : IPropertyModel
	{
		protected Subject<Exception> OnErrorSubject { get; }
		protected Subject<IPropertyViewModel> ShowDetailSubject { get; }

		public TProperty Property { get; protected set; }
		public ReactiveProperty<string> Title => Property.Title;
		public ReactiveProperty<object> Value => Property.Value;
		public ReactiveProperty<string> FormatedString { get; protected set; }
		public abstract IObservable<Unit> OnChanged { get; }
		public IObservable<Exception> OnError => Property.OnError.Merge(OnErrorSubject);
		public IObservable<IPropertyViewModel> ShowDetail => ShowDetailSubject;

		public PropertyViewModel(TProperty property)
		{
			OnErrorSubject = new Subject<Exception>();
			ShowDetailSubject = new Subject<IPropertyViewModel>();
			Property = property;

			var nullString = Value.Where(x => x == null)
				.Select(x => "<null>");

			FormatedString = Value.Where(x => x != null)
				.Select(x =>
				{
					try
					{
						return x?.ToString();
					}
					catch (Exception e)
					{
						OnErrorSubject.OnNext(e);
						return "<表示エラー>";
					}
				})
				.Merge(nullString)
				.ToReactiveProperty("");
		}

		public override string ToString()
		{
			try
			{
				return $"<{GetType()}: {Value}>";
			}
			catch (Exception e)
			{
				OnErrorSubject.OnNext(e);
				return "<表示エラー>";
			}
		}
	}
}
