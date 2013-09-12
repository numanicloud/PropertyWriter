using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PropertyWriter
{
	class DelegateCommand : ICommand
	{
		public Action<object> ExecuteHandler;
		public Func<object, bool> CanExecuteHandler;
		public event EventHandler CanExecuteChanged;

		public bool CanExecute( object parameter )
		{
			if( CanExecuteHandler != null )
			{
				return CanExecuteHandler( parameter );
			}
			return true;
		}

		public void Execute( object parameter )
		{
			if( ExecuteHandler != null )
			{
				ExecuteHandler( parameter );
			}
		}

		public void RaiseCanExecuteChanged()
		{
			var handler = CanExecuteChanged;
			if( handler != null )
			{
				handler( this, null );
			}
		}
	}
}
