using Reactive.Bindings;
using System;
using System.Threading.Tasks;

namespace PropertyWriter.Models.Editor
{
	internal class CleanState : EditorState
	{
		public ReactiveProperty<string> ProjectPath { get; } = new ReactiveProperty<string>();
		public override string Title => $" - {ProjectPath.Value}";
		public override bool CanSave => true;

		public CleanState(Editor manager, string projectPath)
			: base(manager)
		{
			ProjectPath.Value = projectPath;
			CanClose.Value = true;
		}

		public override async Task NewAsync()
		{
			var result = await Manager.CreateNewProjectAsync();
			if (result)
			{
				Manager.State.Value = new NewState(Manager);
			}
		}

		public override async Task OpenAsync()
		{
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
		}

		public override async Task SaveAsAsync()
		{
			await Manager.SaveFileAsAsync();
		}

		public override Task ModifyAsync()
		{
			Manager.State.Value = new DirtyState(Manager, ProjectPath.Value);
			return Task.CompletedTask;
		}

		public override Task CloseAsync()
		{
			throw new NotImplementedException();
		}
	}
}