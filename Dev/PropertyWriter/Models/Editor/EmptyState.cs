using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Models.Editor
{
	class EmptyState : EditorState
	{
		public override string Title => "";
		public override bool CanSave => false;

		public EmptyState(Editor manager)
			: base(manager)
		{
			CanClose.Value = true;
		}

		public override Task ModifyAsync()
		{
            return Task.CompletedTask;
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

		public override Task SaveAsync()
		{
			throw new InvalidOperationException();
		}

		public override Task SaveAsAsync()
		{
			throw new InvalidOperationException();
		}

		public override Task CloseAsync()
		{
			throw new NotImplementedException();
		}
	}
}
