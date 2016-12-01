using Livet.Messaging.Windows;
using PropertyWriter.Annotation;
using PropertyWriter.Models;
using PropertyWriter.Models.Serialize;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PropertyWriter.ViewModels
{
	class ProjectTypeViewModel : Livet.ViewModel
	{
		private readonly Project sandBox_;
		private readonly Project targetProject_;
		public ReactiveProperty<string> Message { get; } = new ReactiveProperty<string>();
		public ReactiveProperty<string> AssemblyPath => sandBox_.AssemblyPath;
		public ReactiveProperty<string> ProjectTypeName => sandBox_.ProjectTypeName;
		public ReactiveProperty<string> ExportPath => sandBox_.SavePath;
		public ReactiveProperty<bool> IsValid => sandBox_.IsValid;
		public ReactiveProperty<bool> Confirmed { get; } = new ReactiveProperty<bool>(false);
		public ReactiveCommand OpenAssemblyCommand { get; } = new ReactiveCommand();
		public ReactiveCommand SelectExportPathCommand { get; } = new ReactiveCommand();
		public ReactiveCommand ConfirmCommand { get; set; }

		public ReactiveProperty<IEnumerable<Type>> AvailableProjectTypes { get; } = new ReactiveProperty<IEnumerable<Type>>();
		public ReactiveProperty<Type> ProjectType { get; } = new ReactiveProperty<Type>();

		public ProjectTypeViewModel(Project project, string message)
		{
			sandBox_ = new Project(project);
			targetProject_ = project;
			Message.Value = message + "アセンブリとプロジェクト型を設定しなおしてください。";

			AssemblyPath.Where(x => x != null && File.Exists(x))
				.Subscribe(x => UpdateAvailableProjectTypes());
			ProjectType.Subscribe(x => sandBox_.ProjectTypeName.Value = x?.Name);

			OpenAssemblyCommand.Subscribe(x => OpenAssembly());
			SelectExportPathCommand.Subscribe(x => SelectExportPath());
			ConfirmCommand = IsValid.ToReactiveCommand();
			ConfirmCommand.SelectMany(x => Confirm().ToObservable())
				.SafelySubscribe(ex => { });
		}

		private void UpdateAvailableProjectTypes()
		{
			AvailableProjectTypes.Value = sandBox_.GetAssembly()
				.GetTypes()
				.Where(y => y.GetCustomAttribute<PwProjectAttribute>() != null)
				.ToArray();
		}

		private async Task Confirm()
		{
			try
			{
				await sandBox_.LoadDataAsync();
			}
			catch (Models.Exceptions.PwObjectMissmatchException)
			{
				Messenger.Raise(new Livet.Messaging.ConfirmationMessage(
					"プロジェクト型情報が一致しません。元の型と同じ型を指定してください。",
					"エラー"));
				return;
			}

			targetProject_.AssemblyPath.Value = sandBox_.AssemblyPath.Value;
			targetProject_.ProjectTypeName.Value = sandBox_.ProjectTypeName.Value;
			targetProject_.SavePath.Value = sandBox_.SavePath.Value;
			targetProject_.Root.Value = sandBox_.Root.Value;
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
