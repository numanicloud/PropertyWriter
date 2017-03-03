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
using PropertyWriter.Models.Editor;

namespace PropertyWriter.ViewModels
{
	class MainViewModel : Livet.ViewModel, IEditorViewModel
	{
		public ReactiveProperty<string> Title { get; private set; }
		public ReactiveProperty<IPropertyViewModel[]> Masters { get; }
		public ReactiveProperty<Editor> Editor { get; } = new ReactiveProperty<Editor>();

		public ReactiveCommand NewProjectCommand { get; } = new ReactiveCommand();
		public ReactiveCommand OpenProjectCommand { get; } = new ReactiveCommand();
		public ReactiveCommand SaveCommand { get; }
		public ReactiveCommand SaveAsCommand { get; }
		public ReactiveCommand CloseCanceledCommand { get; } = new ReactiveCommand();
		public ReactiveCommand ProjectSettingCommand { get; }

		public MainViewModel()
		{
			Editor.Value = new Editor(this);

			Masters = Editor.Value.Project.Where(x => x != null)
                .SelectMany(x => x.Root)
                .Where(x => x != null)
				.Select(x => x.Structure.Properties)
				.Select(x =>
				{
					var factory = new ViewModelFactory(Editor.Value.Project.Value.Factory);
					return x.Select(y => factory.Create(y, true)).ToArray();
				})
				.ToReactiveProperty();
			Title = Editor.SelectMany(x => x.Title)
				.ToReactiveProperty();
			
			SaveCommand = Editor.SelectMany(x => x.CanSave)
				.ToReactiveCommand();
			SaveAsCommand = Editor.SelectMany(x => x.CanSave)
				.ToReactiveCommand();
			ProjectSettingCommand = Editor.Value.Project
				.Select(x => x != null)
				.ToReactiveCommand();
			SubscribeCommands();

			Masters.Where(xs => xs != null).Subscribe(xs =>
			{
				Observable.Merge(xs.Select(x => x.OnChanged))
					.PublishTask(x => Editor.Value.ModifyAsync(), e => ShowError(e, "エラー"));
			});
            Editor.Value.OnSettingChanged
				.PublishTask(x => Editor.Value.ModifyAsync(), e => ShowError(e, "エラー"));

			Masters.Where(x => x != null)
				.SelectMany(x => Observable.Merge(x.Select(y => y.OnError)))
				.Subscribe(err => ShowError(err, "エラー"));
		}

		private void OpenProjectSetting()
		{
            var vm = new ProjectSetting.ProjectSettingViewModel(Editor.Value.Project.Value);
			Messenger.Raise(new TransitionMessage(vm, TransitionMode.Modal, "ProjectSetting"));
		}
		
		private void SubscribeCommands()
		{
			ProjectSettingCommand.Subscribe(x => OpenProjectSetting());

			NewProjectCommand.PublishTask(x => Editor.Value.NewProjectAsync(),
				e => ShowError(e, "プロジェクトの作成に失敗しました。"));
			OpenProjectCommand.PublishTask(x => Editor.Value.OpenAsync(),
				e => ShowError(e, "データを読み込めませんでした。"));
			SaveCommand.PublishTask(x => Editor.Value.SaveAsync(),
				e => ShowError(e, "保存を中止し、以前のファイルに復元しました。"));
			SaveAsCommand.PublishTask(x => Editor.Value.SaveAsAsync(),
				e => ShowError(e, "保存を中止しました。"));
			CloseCanceledCommand.PublishTask(x => Editor.Value.CloseProjectAsync(),
				e => ShowError(e, "ウィンドウを閉じることができませんでした。"));
		}

		private IObservable<Unit> ShowError(Exception exception, string message)
		{
			var vm = new ErrorViewModel(message, exception);
			Messenger.Raise(new TransitionMessage(vm, "Error"));
			return Observable.Empty<Unit>();
		}

		public (bool isCommited, Project result) CreateNewProject()
		{
			var vm = new ProjectSetting.NewProjectViewModel();
			Messenger.Raise(new TransitionMessage(vm, TransitionMode.Modal, "NewProject"));
			return (vm.IsCommitted.Value, vm.Project);
		}

		public (bool isCommited, Project result) RepairProject(Project project, string message)
		{
			var vm = new ProjectSetting.ProjectRepairViewModel(project, message);
			Messenger.Raise(new TransitionMessage(vm, TransitionMode.Modal, "MissingProjectType"));
			return (vm.IsCommitted.Value, vm.Result);
		}

		public ClosingResult ConfirmCloseProject()
		{
			var vm = new ClosingViewModel();
			var message = new TransitionMessage(vm, "ConfirmClose");
			Messenger.Raise(message);
			return vm.Response;
		}

		public async Task TerminateAsync()
		{
			await DispatcherHelper.UIDispatcher.InvokeAsync(() =>
			{
				Messenger.Raise(new WindowActionMessage(WindowAction.Close, "WindowAction"));
			});
		}
	}
}
