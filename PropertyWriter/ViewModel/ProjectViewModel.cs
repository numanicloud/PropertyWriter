using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Livet.Messaging.Windows;
using PropertyWriter.Annotation;
using PropertyWriter.Model;
using Reactive.Bindings;

namespace PropertyWriter.ViewModel
{
	class ProjectViewModel : Livet.ViewModel
	{
		private readonly Project project_;
		public ReactiveProperty<string> AssemblyPath => project_.AssemblyPath;
		public ReactiveProperty<string> ProjectTypeName => project_.ProjectTypeName;
		public ReactiveProperty<string> ExportPath => project_.SavePath;
		public ReactiveProperty<bool> IsValid => project_.IsValid;
		public ReactiveProperty<bool> Confirmed { get; } = new ReactiveProperty<bool>(false);
		public ReactiveCommand OpenAssemblyCommand { get; } = new ReactiveCommand();
		public ReactiveCommand SelectExportPathCommand { get; } = new ReactiveCommand();
		public ReactiveCommand ConfirmCommand { get; set; }

		public ReactiveProperty<IEnumerable<Type>> AvailableProjectTypes { get; } = new ReactiveProperty<IEnumerable<Type>>();
		public ReactiveProperty<Type> ProjectType { get; } = new ReactiveProperty<Type>();

		public ProjectViewModel(Project project)
		{
			project_ = project;

			ProjectType.Subscribe(x => project_.ProjectTypeName.Value = x?.Name);
			OpenAssemblyCommand.Subscribe(x => OpenAssembly());
			SelectExportPathCommand.Subscribe(x => SelectExportPath());

			ConfirmCommand = IsValid.ToReactiveCommand();
			ConfirmCommand.Subscribe(x => Confirm());
		}

		private void Confirm()
		{
			Confirmed.Value = true;
			Messenger.Raise(new WindowActionMessage(WindowAction.Close));
		}

		private void OpenAssembly()
		{
			var dialog = new OpenFileDialog
			{
				FileName = "",
				Filter = "アセンブリ ファイル (*.dll, *.exe)|*.dll;*.exe",
				Title = "アセンブリを開く"
			};
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				AssemblyPath.Value = dialog.FileName;
				AvailableProjectTypes.Value = project_.GetAssembly().GetTypes()
					.Where(x => x.GetCustomAttribute<PwProjectAttribute>() != null)
					.ToArray();
			}
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
				ExportPath.Value = dialog.FileName;
			}
		}
	}
}
