using System;
using Livet.Messaging.Windows;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;

namespace PropertyWriter.ViewModels
{
	class BlockViewModel : Livet.ViewModel
	{
		public ReactiveProperty<IPropertyModel> Model { get; set; } = new ReactiveProperty<IPropertyModel>();
		public ReactiveProperty<string> Title => Model.Value.Title;

		public ReactiveCommand CloseCommand { get; private set; } = new ReactiveCommand();

		public BlockViewModel(IPropertyModel model)
		{
			Model.Value = model;
			CloseCommand.Subscribe(x => Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close")));
		}
	}
}
