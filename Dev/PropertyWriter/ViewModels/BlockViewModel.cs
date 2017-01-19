using System;
using Livet.Messaging.Windows;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;
using PropertyWriter.ViewModels.Properties.Common;
using Livet.Messaging;

namespace PropertyWriter.ViewModels
{
	class BlockViewModel : Livet.ViewModel
	{
		public ReactiveProperty<IPropertyViewModel> Model { get; set; } = new ReactiveProperty<IPropertyViewModel>();
		public ReactiveProperty<string> Title => Model.Value.Title;

		public ReactiveCommand CloseCommand { get; private set; } = new ReactiveCommand();

		public BlockViewModel(IPropertyViewModel model)
		{
			Model.Value = model;
			CloseCommand.Subscribe(x => Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close")));
			Model.Value.OnError.Subscribe(e => ShowError(e, "エラー"));
		}

		private void ShowError(Exception exception, string message)
		{
			var vm = new ErrorViewModel(message, exception);
			Messenger.Raise(new TransitionMessage(vm, "Error"));
		}
	}
}
