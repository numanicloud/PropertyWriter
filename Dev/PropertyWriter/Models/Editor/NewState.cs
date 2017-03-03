using System;
using System.Threading.Tasks;

namespace PropertyWriter.Models.Editor
{
	internal class NewState : EditorState
	{
		public override string Title => " - 新規プロジェクト";
		public override bool CanSave => true;

		public NewState(Editor manager)
			: base(manager)
		{
			CanClose.Value = false;
		}

		public override Task ModifyAsync()
		{
			return Task.CompletedTask;
		}

		public override async Task NewAsync()
		{
			switch (Manager.ConfirmClose())
			{
			case ClosingResult.Anyway:
				break;

			case ClosingResult.AfterSave:
				await Manager.SaveFileAsAsync();
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
				await Manager.SaveFileAsAsync();
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
			var path = await Manager.SaveFileAsAsync();
			if (path != null)
			{
				Manager.State.Value = new CleanState(Manager, path);
			}
		}

		public override async Task SaveAsAsync()
		{
			var path = await Manager.SaveFileAsAsync();
			if (path != null)
			{
				Manager.State.Value = new CleanState(Manager, path);
			}
		}

		public override async Task CloseAsync()
		{
			switch (Manager.ConfirmClose())
			{
			case ClosingResult.Anyway:
				break;

			case ClosingResult.AfterSave:
				var result = await Manager.SaveFileAsAsync();
				if (result == null)
				{
					return;
				}
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