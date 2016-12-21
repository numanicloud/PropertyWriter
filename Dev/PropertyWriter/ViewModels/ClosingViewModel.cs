using Livet.Messaging.Windows;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.ViewModels
{
	class ClosingViewModel : Livet.ViewModel
	{
		public enum Result
		{
			Anyway, AfterSave, Cancel,
		}

		public Result Response { get; private set; }
		public ReactiveCommand AnywayCommand { get; } = new ReactiveCommand();
		public ReactiveCommand SaveCommand { get; } = new ReactiveCommand();
		public ReactiveCommand CancelCommand { get; } = new ReactiveCommand();

		public ClosingViewModel()
		{
			void SubscribeCommand(ReactiveCommand command, Result result)
			{
				command.Subscribe(x =>
				{
					Response = result;
					Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));
				});
			}

			Response = Result.Cancel;
			SubscribeCommand(AnywayCommand, Result.Anyway);
			SubscribeCommand(SaveCommand, Result.AfterSave);
			SubscribeCommand(CancelCommand, Result.Cancel);
		}
	}
}
