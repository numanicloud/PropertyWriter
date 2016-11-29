using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Forms;
using Livet.Messaging;
using PropertyWriter.Models;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;
using PropertyWriter.ViewModels.Properties.Common;
using System.Diagnostics;
using Livet;
using Livet.Messaging.Windows;

namespace PropertyWriter.ViewModels
{
	class MainViewModel : Livet.ViewModel
	{
		public ReactiveProperty<string> StatusMessage { get; private set; } = new ReactiveProperty<string>();
		public ReactiveProperty<bool> IsError { get; private set; } = new ReactiveProperty<bool>();
		public ReactiveProperty<string> Title { get; private set; }
		public ReactiveProperty<string> ProjectPath { get; private set; } = new ReactiveProperty<string>(mode:ReactivePropertyMode.None);
		public ReactiveProperty<bool> IsReady { get; private set; } = new ReactiveProperty<bool>();
		public ReactiveProperty<bool> IsModified { get; private set; } = new ReactiveProperty<bool>();
		public ReactiveProperty<bool> CanClose { get; private set; } = new ReactiveProperty<bool>();

		public ReactiveProperty<Project> Project { get; } = new ReactiveProperty<Project>();
		public ReactiveProperty<IPropertyViewModel[]> Masters { get; }  // TODO: ViewModelで差し替え

		public ReactiveCommand NewProjectCommand { get; } = new ReactiveCommand();
		public ReactiveCommand OpenProjectCommand { get; } = new ReactiveCommand();
		public ReactiveCommand SaveCommand { get; }
		public ReactiveCommand CloseCanceledCommand { get; } = new ReactiveCommand();

		public MainViewModel()
		{
			SaveCommand = Project.Select(x => x?.IsValid?.Value == true)
				.ToReactiveCommand();
			Masters = Project.Where(x => x != null)
                .SelectMany(x => x.Root)
                .Where(x => x != null)
				.Select(x => x.Structure.Properties)
				.Select(x => x.Select(ViewModelFactory.Create).ToArray())
				.ToReactiveProperty();

			var projectTitle = ProjectPath.Where(x => IsReady.Value)
				.Select(x => x ?? "新規プロジェクト")
				.Select(x => " - " + x);
			var modifiedTitle = IsModified.Select(x => x ? " - 変更あり" : "");
			Title = Observable.Return("PropertyWriter")
				.CombineLatest(projectTitle, (x, y) => x + y)
				.CombineLatest(modifiedTitle, (x, y) => x + y)
				.ToReactiveProperty("PropertyWriter");
			
			NewProjectCommand.Subscribe(x => CreateNewProject());
			SubscribeOpenCommand();
            SubscribeSaveCommand();
			CloseCanceledCommand.Subscribe(async x => await HandleClose());
			
			Masters.Where(xs => xs != null).Subscribe(xs =>
			{
				Observable.Merge(xs.Select(x => x.OnChanged))
					.Subscribe(x => IsModified.Value = true);
			});

			IsError.Value = false;
			IsReady.Value = false;
			IsModified.Value = false;
			CanClose.Value = false;
		}

		private async Task HandleClose()
		{
			if (IsModified.Value)
			{
				var vm = new ClosingViewModel();
				var message = new TransitionMessage(vm, "ConfirmClose");
				Messenger.Raise(message);

				if (vm.Response == ClosingViewModel.Result.Cancel)
				{
					return;
				}

				if (vm.Response == ClosingViewModel.Result.AfterSave)
				{
					try
					{
						await SaveFileAsync();
					}
					catch (Exception exception)
					{
						Debugger.Log(1, "Error", exception + "\n");
						StatusMessage.Value = $"保存を中止し、以前のファイルに復元しました。{exception.Message}";
						IsError.Value = true;
						SubscribeSaveCommand();
						return;
					}
				}
			}
			CanClose.Value = true;
			await DispatcherHelper.UIDispatcher.InvokeAsync(() =>
			{
				Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));
			});
		}

		private void SubscribeSaveCommand()
        {
            SaveCommand.SelectMany(x => SaveFileAsync().ToObservable())
                .Subscribe(
                    unit => { },
                    exception =>
					{
						Debugger.Log(1, "Error", exception + "\n");
						StatusMessage.Value = $"保存を中止し、以前のファイルに復元しました。{exception.Message}";
                        IsError.Value = true;
                        SubscribeSaveCommand();
                    }); ;
        }

        private void SubscribeOpenCommand()
		{
			OpenProjectCommand.SelectMany(x => OpenProjectAsync().ToObservable())
				.Subscribe(
					unit => { },
					exception =>
					{
						Debugger.Log(1, "Error", exception + "\n");
						StatusMessage.Value = $"データを読み込めませんでした。{exception.Message}";
						IsError.Value = true;
						SubscribeOpenCommand();
					});
		}

		private void CreateNewProject()
		{
			var project = new Project();
			var vm = new NewProjectViewModel(project);
			Messenger.Raise(new TransitionMessage(vm, TransitionMode.Modal, "NewProject"));

			if (vm.Confirmed.Value)
			{
				Project.Value = project;
                Project.Value.Initialize();

				StatusMessage.Value = "プロジェクトを作成しました。";
				IsError.Value = false;
				IsReady.Value = true;
				ProjectPath.Value = null;
			}
		}

		private async Task OpenProjectAsync()
		{
			var dialog = new OpenFileDialog()
			{
				FileName = "",
				Filter = "マスター プロジェクト (*.pwproj)|*.pwproj",
				Title = "マスターデータ プロジェクトを開く"
			};
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				StatusMessage.Value = "プロジェクトを読み込み中…";

                Project.Value = await Models.Project.LoadAsync(dialog.FileName);

				StatusMessage.Value = "プロジェクトを読み込みました。";
				IsError.Value = false;
				IsReady.Value = true;
				IsModified.Value = false;
				ProjectPath.Value = dialog.FileName;
			}
		}

		private async Task SaveFileAsync()
		{
			if (ProjectPath.Value == null)
			{
				var dialog = new SaveFileDialog()
				{
					FileName = "NewProject.pwproj",
					Filter = "マスター プロジェクト (*.pwproj)|*.pwproj",
					Title = "マスター プロジェクトを保存"
				};
				if(dialog.ShowDialog() == DialogResult.OK)
				{
					ProjectPath.Value = dialog.FileName;
				}
			}

			if (ProjectPath.Value == null)
			{
				return;
			}

			StatusMessage.Value = "データを保存中…";
            
            await Project.Value.SaveAsync(ProjectPath.Value);

			StatusMessage.Value = "データを保存しました。";
			IsError.Value = false;
			IsModified.Value = false;
		}
	}
}
