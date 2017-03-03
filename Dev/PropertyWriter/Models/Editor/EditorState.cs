using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PropertyWriter.Models.Editor
{
	abstract class EditorState
	{
		protected Editor Manager { get; }

		public abstract string Title { get; }
		public abstract bool CanSave { get; }
		public ReactiveProperty<bool> CanClose { get; } = new ReactiveProperty<bool>(true);

		public EditorState(Editor manager)
		{
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
