using System.Reflection;
using Reactive.Bindings;
using System;
using System.Reactive.Subjects;
using System.Diagnostics;

namespace PropertyWriter.Models.Properties.Interfaces
{
    public abstract class PropertyModel : IPropertyModel
    {
		protected Subject<Exception> OnErrorSubject { get; } = new Subject<Exception>();

        public abstract ReactiveProperty<object> Value { get; }
		public abstract Type ValueType { get; }
        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>();
        public PropertyInfo PropertyInfo { get; set; }
		public IObservable<Exception> OnError => OnErrorSubject;

		public PropertyModel()
		{
			OnError.Subscribe(x => Debugger.Log(1, "Error", $"Error from {Title.Value}\n"));
		}
    }
}
