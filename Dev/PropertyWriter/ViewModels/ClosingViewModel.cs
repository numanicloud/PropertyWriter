using Livet.Messaging.Windows;
using PropertyWriter.Models.Editor;
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
		public ClosingResult Response { get; private set; }
		public ReactiveCommand AnywayCommand { get; } = new ReactiveCommand();
		public ReactiveCommand SaveCommand { get; } = new ReactiveCommand();
		public ReactiveCommand CancelCommand { get; } = new ReactiveCommand();

		public ClosingViewModel()
		{
			void SubscribeCommand(ReactiveCommand command, ClosingResult result)
			{
				command.Subscribe(x =>
				{
					Response = result;
					Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));
				});
			}

			Response = ClosingResult.Cancel;
			SubscribeCommand(AnywayCommand, ClosingResult.Anyway);
			SubscribeCommand(SaveCommand, ClosingResult.AfterSave);
			SubscribeCommand(CancelCommand, ClosingResult.Cancel);
		}
	}
}
