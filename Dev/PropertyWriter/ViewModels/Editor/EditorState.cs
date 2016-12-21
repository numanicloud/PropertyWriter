using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PropertyWriter.ViewModels.Editor
{
	abstract class EditorState
	{
		protected MainViewModel Owner { get; }
		protected EditorLifecycleManager Manager { get; }

		public abstract string Title { get; }
		public abstract bool CanSave { get; }
		public ReactiveProperty<bool> CanClose { get; } = new ReactiveProperty<bool>(true);

		public EditorState(MainViewModel owner, EditorLifecycleManager manager)
		{
			Owner = owner;
			Manager = manager;
		}

		public abstract Task NewAsync();
		public abstract Task OpenAsync();
		public abstract Task SaveAsync();
		public abstract Task SaveAsAsync();
		public abstract Task ModifyAsync();
		public abstract Task CloseAsync();
	}
}
