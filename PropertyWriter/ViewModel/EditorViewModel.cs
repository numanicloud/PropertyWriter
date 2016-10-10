using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model;
using MvvmHelper;
using Reactive.Bindings;

namespace PropertyWriter
{
	class EditorViewModel
	{
		public EditorViewModel()
		{
			Editor = new PropertyEditor();

			NewFileCommand = new DelegateCommand
			{
				ExecuteHandler = OnNewFile,
			};
			AddCommand = new DelegateCommand
			{
				ExecuteHandler = o => AddData(),
				CanExecuteHandler = o => Editor.IsInitialized,
			};
			RemoveCommand = new DelegateCommand
			{
				ExecuteHandler = RemoveData,
				CanExecuteHandler = o => Editor.IsInitialized,
			};
		}

		public ReactiveProperty<int> DataIndex { get; set; } = new ReactiveProperty<int>();

		public DelegateCommand AddCommand { get; private set; }
		public DelegateCommand RemoveCommand { get; private set; }
		public DelegateCommand NewFileCommand { get; private set; }
		public DelegateCommand OpenCommand { get; private set; }

		private PropertyEditor Editor { get; set; }

		private void OnNewFile(object obj)
		{
			var dialog = new TypeSelectWindow();
			dialog.ShowDialog();
			if(dialog.TargetType != null)
			{
				Editor.Initialize(dialog.TargetType);
				AddCommand.RaiseCanExecuteChanged();

				DataIndex.Value = -1;
			}
		}

		private void AddData()
		{
			var data = Editor.CreateData();
			Editor.AddNewData(data);
		}

		private void RemoveData(object obj)
		{
			Editor.RemoveData(DataIndex.Value);
			DataIndex.Value = -1;
		}
	}
}
