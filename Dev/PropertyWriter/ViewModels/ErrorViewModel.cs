using Livet.Messaging.Windows;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.ViewModels
{
	class ErrorViewModel : Livet.ViewModel
	{
		public ReactiveProperty<string> Message { get; set; }
		public ReactiveProperty<Exception> Exception { get; set; }
		public ReactiveProperty<string> ErrorDescription { get; }
		public ReactiveCommand OkCommand { get; } = new ReactiveCommand();

		public ErrorViewModel(string message, Exception exception)
		{
			Message = new ReactiveProperty<string>(message);
			Exception = new ReactiveProperty<System.Exception>(exception);
			ErrorDescription = Exception.Select(x => x.ToString()).ToReactiveProperty();

			OkCommand.Subscribe(x => Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close")));
		}
	}
}
