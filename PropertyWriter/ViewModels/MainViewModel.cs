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
			Manager.Value = new EditorLifecycleManager(this);
			State.Value = new EmptyState(this, Manager.Value);

			Masters = Manager.Value.Project.Where(x => x != null)
                .SelectMany(x => x.Root)
                .Where(x => x != null)
				.Select(x => x.Structure.Properties)
				.Select(x => x.Select(ViewModelFactory.Create).ToArray())
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
					.SelectMany(x => State.Value.ModifyAsync().ToObservable())
					.SafelySubscribe(ErrorHandler("エラー"));
			});
            Manager.Value.OnSettingChanged
                .SelectMany(x => State.Value.ModifyAsync().ToObservable())
                .SafelySubscribe(ErrorHandler("エラー"));
		}

		private void OpenProjectSetting()
		{
            var vm = new ProjectSetting.ProjectSettingViewModel(Manager.Value.Project.Value);
			Messenger.Raise(new TransitionMessage(vm, TransitionMode.Modal, "ProjectSetting"));
		}
		
		private void SubscribeCommands()
		{
			ProjectSettingCommand.Subscribe(x => OpenProjectSetting());

			NewProjectCommand.SelectMany(x => State.Value.NewAsync().ToObservable())
				.SafelySubscribe(ErrorHandler("プロジェクトの作成に失敗しました。"));

			OpenProjectCommand.SelectMany(x => State.Value.OpenAsync().ToObservable())
				.SafelySubscribe(ErrorHandler("データを読み込めませんでした。"));

			SaveCommand.SelectMany(x => State.Value.SaveAsync().ToObservable())
				.SafelySubscribe(ErrorHandler("保存を中止し、以前のファイルに復元しました。"));

			SaveAsCommand.SelectMany(x => State.Value.SaveAsAsync().ToObservable())
				.SafelySubscribe(ErrorHandler("保存を中止しました。"));

			CloseCanceledCommand.SelectMany(x => State.Value.CloseAsync().ToObservable())
				.SafelySubscribe(ErrorHandler("ウィンドウを閉じることができませんでした。"));
		}

		private Action<Exception> ErrorHandler(string message)
		{
			return exception =>
			{
				Manager.Value.IsError.Value = true;
				Manager.Value.StatusMessage.Value = $"{message} {exception.Message}";
			};
		}
	}
}
