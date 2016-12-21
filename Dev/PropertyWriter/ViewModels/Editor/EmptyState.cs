using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.ViewModels.Editor
{
	class EmptyState : EditorState
	{
		public override string Title => "";
		public override bool CanSave => false;

		public EmptyState(MainViewModel owner, EditorLifecycleManager manager)
			: base(owner, manager)
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
				Owner.State.Value = new NewState(Owner, Manager);
			}
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
