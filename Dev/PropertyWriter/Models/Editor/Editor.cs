using Livet;
using Livet.Messaging;
using Livet.Messaging.Windows;
using PropertyWriter.Models;
using PropertyWriter.Models.Editor;
using PropertyWriter.Models.Properties.Common;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PropertyWriter.Models.Editor
{
	class Editor
	{
		private IEditorViewModel editorViewModel_;

		public ReactiveProperty<Project> Project { get; set; } = new ReactiveProperty<Models.Project>();
		public ReactiveProperty<EditorState> State { get; set; } = new ReactiveProperty<EditorState>();
		public IObservable<Unit> OnSettingChanged { get; }

		public ReactiveProperty<bool> IsError { get; private set; } = new ReactiveProperty<bool>();
		public ReactiveProperty<string> StatusMessage { get; private set; } = new ReactiveProperty<string>();
		public ReactiveProperty<string> Title { get; private set; } = new ReactiveProperty<string>();
		public ReactiveProperty<bool> CanSave { get; private set; } = new ReactiveProperty<bool>();
		public ReactiveProperty<bool> CanClose { get; private set; } = new ReactiveProperty<bool>();

		public Editor(IEditorViewModel owner)
		{
			editorViewModel_ = owner;
			State.Value = new EmptyState(this);

            var project = Project.Where(x => x != null);
			var dependencyChanged = project.SelectMany(x => Observable.Merge(x.DependenciesPathes.ToArray()));
            OnSettingChanged = project.SelectMany(x => x.SavePath)
                .Merge(project.SelectMany(x => x.AssemblyPath))
                .Merge(project.SelectMany(x => x.ProjectTypeName))
                .Merge(dependencyChanged)
                .Select(x => Unit.Default);

			Title = State.Select(x => "PropertyWriter" + x.Title).ToReactiveProperty();
			CanSave = State.Select(x => x.CanSave).ToReactiveProperty();
			CanClose = State.SelectMany(x => x.CanClose).ToReactiveProperty();
		}


		public async Task<bool> CreateNewProjectAsync()
		{
			var (isCommitted, project) = editorViewModel_.CreateNewProject();
			if (isCommitted)
			{
				Project.Value = project;
				await Project.Value.LoadDependencyAsync();
				Project.Value.InitializeRoot(Project.Value.Dependencies.ToArray());

				StatusMessage.Value = "プロジェクトを作成しました。";
				IsError.Value = false;
				return true;
			}

			return false;
		}

		public async Task<(string path, bool isDirtySetting)> OpenProjectAsync()
		{
			var dialog = new OpenFileDialog()
			{
				FileName = "",
				Filter = "マスター プロジェクト (*.pwproj)|*.pwproj",
				Title = "マスターデータ プロジェクトを開く"
			};
            bool isDirty = false;

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				StatusMessage.Value = "プロジェクトを読み込み中…";

				var project = await Models.Project.LoadSettingAsync(dialog.FileName);
				try
				{
					await project.LoadDataAsync();
				}
				catch (Models.Exceptions.PwProjectException ex)
				{
					var (isCommitted, result) = editorViewModel_.RepairProject(project, ex.Message);
					if (!isCommitted)
					{
						return (null, false);
					}
                    project = result;
                    isDirty = true;
				}
				catch(Exception)
				{
					StatusMessage.Value = "プロジェクトを読み込めませんでした。";
					IsError.Value = true;
					throw;
				}

				Project.Value = project;
				StatusMessage.Value = "プロジェクトを読み込みました。";
				IsError.Value = false;
				return (dialog.FileName, isDirty);
			}
			return (null, false);
		}

		public async Task SaveFileAsync(string path)
		{
			StatusMessage.Value = "データを保存中…";

			await Project.Value.SaveSettingAsync(path);
			await Project.Value.SaveSerializedDataAsync();

			StatusMessage.Value = "データを保存しました。";
			IsError.Value = false;
		}

		public Task OpenAsync() => State.Value.OpenAsync();
		public Task SaveAsync() => State.Value.SaveAsync();
		public Task SaveAsAsync() => State.Value.SaveAsAsync();
		public Task NewProjectAsync() => State.Value.NewAsync();
		public Task CloseProjectAsync() => State.Value.CloseAsync();
		public Task ModifyAsync() => State.Value.ModifyAsync();

		public async Task<string> SaveFileAsAsync()
		{
			var dialog = new SaveFileDialog()
			{
				FileName = "NewProject.pwproj",
				Filter = "マスター プロジェクト (*.pwproj)|*.pwproj",
				Title = "マスター プロジェクトを保存"
			};

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				StatusMessage.Value = "データを保存中…";

				await Project.Value.SaveSettingAsync(dialog.FileName);
				await Project.Value.SaveSerializedDataAsync();

				StatusMessage.Value = "データを保存しました。";
				IsError.Value = false;
				return dialog.FileName;
			}

			return null;
		}

		public ClosingResult ConfirmClose()
		{
			return editorViewModel_.ConfirmCloseProject();
		}

		public async Task CloseAsync()
		{
			await editorViewModel_.TerminateAsync();
		}
	}
}
