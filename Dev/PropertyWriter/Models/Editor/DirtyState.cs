using System;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace PropertyWriter.Models.Editor
{
	internal class DirtyState : EditorState
	{
		public ReactiveProperty<string> ProjectPath { get; } = new ReactiveProperty<string>();
		public override string Title => $" - {ProjectPath.Value} - 変更あり";
		public override bool CanSave => true;

		public DirtyState(Editor manager, string projectPath)
			: base(manager)
		{
			ProjectPath.Value = projectPath;
			CanClose.Value = false;
		}

		public override async Task NewAsync()
		{
			switch (Manager.ConfirmClose())
			{
			case ClosingResult.Anyway:
				break;

			case ClosingResult.AfterSave:
				await Manager.SaveFileAsync(ProjectPath.Value);
				break;

			case ClosingResult.Cancel:
			default:
				return;
			}
			
			var result = await Manager.CreateNewProjectAsync();
			if (result)
			{
				Manager.State.Value = new NewState(Manager);
			}
		}

		public override async Task OpenAsync()
		{
			switch (Manager.ConfirmClose())
			{
			case ClosingResult.Anyway:
				break;

			case ClosingResult.AfterSave:
				await Manager.SaveFileAsync(ProjectPath.Value);
				break;

			case ClosingResult.Cancel:
			default:
				return;
			}
			
			var result = await Manager.OpenProjectAsync();
			if (result.path != null)
			{
                if (result.isDirtySetting)
                {
                    Manager.State.Value = new DirtyState(Manager, result.path);
                }
                else
                {
                    Manager.State.Value = new CleanState(Manager, result.path);
                }
			}
		}

		public override async Task SaveAsync()
		{
			await Manager.SaveFileAsync(ProjectPath.Value);
			Manager.State.Value = new CleanState(Manager, ProjectPath.Value);
		}

		public override async Task SaveAsAsync()
		{
			var path = await Manager.SaveFileAsAsync();
			if (path != null)
			{
				Manager.State.Value = new CleanState(Manager, path);
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
			case ClosingResult.Anyway:
				break;

			case ClosingResult.AfterSave:
				await Manager.SaveFileAsync(ProjectPath.Value);
				break;

			case ClosingResult.Cancel:
			default:
				return;
			}

			CanClose.Value = true;
			await Manager.CloseAsync();
		}
	}
}