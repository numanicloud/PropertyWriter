using Reactive.Bindings;
using System;
using System.Threading.Tasks;

namespace PropertyWriter.ViewModels.Editor
{
	internal class CleanState : EditorState
	{
		public ReactiveProperty<string> ProjectPath { get; } = new ReactiveProperty<string>();
		public override string Title => $" - {ProjectPath.Value}";
		public override bool CanSave => true;

		public CleanState(MainViewModel owner, EditorLifecycleManager manager, string projectPath)
			: base(owner, manager)
		{
			ProjectPath.Value = projectPath;
			CanClose.Value = true;
		}

		public override Task NewAsync()
		{
			var result = Manager.CreateNewProject();
			if (result)
			{
				Owner.State.Value = new NewState(Owner, Manager);
			}
			return Task.CompletedTask;
		}

		public override async Task OpenAsync()
		{
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
		}

		public override async Task SaveAsAsync()
		{
			await Manager.SaveFileAsAsync();
		}

		public override Task ModifyAsync()
		{
			Owner.State.Value = new DirtyState(Owner, Manager, ProjectPath.Value);
			return Task.CompletedTask;
		}

		public override Task CloseAsync()
		{
			throw new NotImplementedException();
		}
	}
}