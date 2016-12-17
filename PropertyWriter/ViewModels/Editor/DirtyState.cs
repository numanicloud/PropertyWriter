using System;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace PropertyWriter.ViewModels.Editor
{
	internal class DirtyState : EditorState
	{
		public ReactiveProperty<string> ProjectPath { get; } = new ReactiveProperty<string>();
		public override string Title => $" - {ProjectPath.Value} - 変更あり";
		public override bool CanSave => true;

		public DirtyState(MainViewModel owner, EditorLifecycleManager manager, string projectPath)
			: base(owner, manager)
		{
			ProjectPath.Value = projectPath;
			CanClose.Value = false;
		}

		public override async Task NewAsync()
		{
			switch (Manager.ConfirmClose())
			{
			case ClosingViewModel.Result.Anyway:
				break;

			case ClosingViewModel.Result.AfterSave:
				await Manager.SaveFileAsync(ProjectPath.Value);
				break;

			case ClosingViewModel.Result.Cancel:
			default:
				return;
			}
			
			var result = Manager.CreateNewProject();
			if (result)
			{
				Owner.State.Value = new NewState(Owner, Manager);
			}
		}

		public override async Task OpenAsync()
		{
			switch (Manager.ConfirmClose())
			{
			case ClosingViewModel.Result.Anyway:
				break;

			case ClosingViewModel.Result.AfterSave:
				await Manager.SaveFileAsync(ProjectPath.Value);
				break;

			case ClosingViewModel.Result.Cancel:
			default:
				return;
			}
			
			var result = await Manager.OpenProjectAsync();
			if (result.path != null)
			{
                if (result.isDirtySetting)
                {
                    Owner.State.Value = new DirtyState(Owner, Manager, result.path);
                }
                else
                {
                    Owner.State.Value = new CleanState(Owner, Manager, result.path);
                }
			}
		}

		public override async Task SaveAsync()
		{
			await Manager.SaveFileAsync(ProjectPath.Value);
			Owner.State.Value = new CleanState(Owner, Manager, ProjectPath.Value);
		}

		public override async Task SaveAsAsync()
		{
			var path = await Manager.SaveFileAsAsync();
			if (path != null)
			{
				Owner.State.Value = new CleanState(Owner, Manager, path);
			}
		}

		public override Task ModifyAsync()
		{
			return Task.CompletedTask;
		}

		public override async Task CloseAsync()
		{
			switch (Manager.ConfirmClose())
			{
			case ClosingViewModel.Result.Anyway:
				break;

			case ClosingViewModel.Result.AfterSave:
				await Manager.SaveFileAsync(ProjectPath.Value);
				break;

			case ClosingViewModel.Result.Cancel:
			default:
				return;
			}

			await Manager.CloseAsync();
		}
	}
}