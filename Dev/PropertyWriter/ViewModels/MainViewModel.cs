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
using PropertyWriter.ViewModels.Editor;

namespace PropertyWriter.ViewModels
{
	class MainViewModel : Livet.ViewModel
	{
		public ReactiveProperty<string> Title { get; private set; }
		public ReactiveProperty<IPropertyViewModel[]> Masters { get; }
		public ReactiveProperty<EditorState> State { get; } = new ReactiveProperty<EditorState>();
		public ReactiveProperty<EditorLifecycleManager> Manager { get; } = new ReactiveProperty<EditorLifecycleManager>();

		public ReactiveCommand NewProjectCommand { get; } = new ReactiveCommand();
		public ReactiveCommand OpenProjectCommand { get; } = new ReactiveCommand();
		public ReactiveCommand SaveCommand { get; }
		public ReactiveCommand SaveAsCommand { get; }
		public ReactiveCommand CloseCanceledCommand { get; } = new ReactiveCommand();
		public ReactiveCommand ProjectSettingCommand { get; }

		public MainViewModel()
		{
			var factory = new ViewModelFactory();

			Manager.Value = new EditorLifecycleManager(this);
			State.Value = new EmptyState(this, Manager.Value);

			Masters = Manager.Value.Project.Where(x => x != null)
                .SelectMany(x => x.Root)
                .Where(x => x != null)
				.Select(x => x.Structure.Properties)
				.Select(x => x.Select(y => factory.Create(y, true)).ToArray())
				.ToReactiveProperty();
			Title = State.Select(x => "PropertyWriter" + x.Title)
				.ToReactiveProperty();
			
			SaveCommand = State.Select(x => x.CanSave)
				.ToReactiveCommand();
			SaveAsCommand = State.Select(x => x.CanSave)
				.ToReactiveCommand();
			ProjectSettingCommand = Manager.Value.Project
				.Select(x => x != null)
				.ToReactiveCommand();
			SubscribeCommands();

			Masters.Where(xs => xs != null).Subscribe(xs =>
			{
				Observable.Merge(xs.Select(x => x.OnChanged))
					.PublishTask(x => State.Value.ModifyAsync(), e => ShowError(e, "エラー"));
			});
            Manager.Value.OnSettingChanged
				.PublishTask(x => State.Value.ModifyAsync(), e => ShowError(e, "エラー"));

			Masters.Where(x => x != null)
				.SelectMany(x => Observable.Merge(x.Select(y => y.OnError)))
				.Subscribe(err => ShowError(err, "エラー"));
		}

		private void OpenProjectSetting()
		{
            var vm = new ProjectSetting.ProjectSettingViewModel(Manager.Value.Project.Value);
			Messenger.Raise(new TransitionMessage(vm, TransitionMode.Modal, "ProjectSetting"));
		}
		
		private void SubscribeCommands()
		{
			ProjectSettingCommand.Subscribe(x => OpenProjectSetting());

			NewProjectCommand.PublishTask(x => State.Value.NewAsync(),
				e => ShowError(e, "プロジェクトの作成に失敗しました。"));
			OpenProjectCommand.PublishTask(x => State.Value.OpenAsync(),
				e => ShowError(e, "データを読み込めませんでした。"));
			SaveCommand.PublishTask(x => State.Value.SaveAsync(),
				e => ShowError(e, "保存を中止し、以前のファイルに復元しました。"));
			SaveAsCommand.PublishTask(x => State.Value.SaveAsAsync(),
				e => ShowError(e, "保存を中止しました。"));
			CloseCanceledCommand.PublishTask(x => State.Value.CloseAsync(),
				e => ShowError(e, "ウィンドウを閉じることができませんでした。"));
		}

		private IObservable<Unit> ShowError(Exception exception, string message)
		{
			var vm = new ErrorViewModel(message, exception);
			Messenger.Raise(new TransitionMessage(vm, "Error"));
			return Observable.Empty<Unit>();
		}
	}
}
