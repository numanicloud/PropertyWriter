using PropertyWriter.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Livet.Messaging.Windows;

namespace PropertyWriter.ViewModels
{
	class OutputPathViewModel : Livet.ViewModel
	{
		public ReactiveProperty<Project> Project { get; set; } = new ReactiveProperty<Project>();
		public ReactiveProperty<string> OutputPath { get; } = new ReactiveProperty<string>();
		public ReactiveCommand ReferenceCommand { get; } = new ReactiveCommand();
		public ReactiveCommand ApplyCommand { get; } = new ReactiveCommand();

		public OutputPathViewModel(Project project)
		{
			Project.Value = project;
			OutputPath.Value = project.SavePath.Value;
			ReferenceCommand.Subscribe(x => SelectExportPath());
			ApplyCommand.Subscribe(x => Apply());
		}

		private void Apply()
		{
			Project.Value.SavePath.Value = OutputPath.Value;
			Messenger.Raise(new WindowActionMessage(WindowAction.Close));
		}

		private void SelectExportPath()
		{
			var dialog = new SaveFileDialog()
			{
				FileName = "DataBase.json",
				Filter = "任意の種類 (*.*)|*.*",
				Title = "アセンブリを開く"
			};
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				OutputPath.Value = dialog.FileName;
			}
		}
	}
}
