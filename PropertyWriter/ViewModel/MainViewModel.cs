using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model;
using MvvmHelper;
using System.ComponentModel;
using Reactive.Bindings;

namespace PropertyWriter.ViewModel
{
	class MainViewModel
	{
		public ReactiveProperty<IInstance> Root { get; } = new ReactiveProperty<IInstance>();

		public MainViewModel()
		{
			NewFileCommand = new DelegateCommand
			{
				ExecuteHandler = OnNewFile,
			};
		}

		public DelegateCommand NewFileCommand { get; set; }

		private void OnNewFile(object obj)
		{
			var dialog = new TypeSelectWindow();
			dialog.ShowDialog();
			if(dialog.TargetType != null)
			{
				var type = typeof(IEnumerable<>).MakeGenericType(dialog.TargetType);
				Root.Value = InstanceFactory.Create(type);
			}
		}
	}
}
