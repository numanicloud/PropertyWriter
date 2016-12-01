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
			throw new InvalidOperationException();
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
			var path = await Manager.OpenProjectAsync();
			if (path != null)
			{
				Owner.State.Value = new CleanState(Owner, Manager, path);
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
