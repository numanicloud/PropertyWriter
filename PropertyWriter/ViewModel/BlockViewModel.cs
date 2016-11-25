using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet.Messaging.Windows;
using PropertyWriter.Model.Instance;
using Reactive.Bindings;

namespace PropertyWriter.ViewModel
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
		}
	}
}
