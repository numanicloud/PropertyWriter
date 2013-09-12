using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model;
using MvvmHelper;
using System.ComponentModel;

namespace PropertyWriter.ViewModel
{
	class MainViewModel : INotifyPropertyChanged
	{
		public MainViewModel()
		{
			NewFileCommand = new DelegateCommand
			{
				ExecuteHandler = OnNewFile,
			};
		}

		#region Root

		public IInstance Root
		{
			get { return _Root; }
			set
			{
				_Root = value;
				PropertyChanged.Raise( this, RootName );
			}
		}

		private IInstance _Root;
		internal static readonly string RootName = PropertyName<MainViewModel>.Get( _ => _.Root );

		#endregion

		public DelegateCommand NewFileCommand { get; set; }

		private void OnNewFile( object obj )
		{
			var dialog = new TypeSelectWindow();
			dialog.ShowDialog();
			if( dialog.TargetType != null )
			{
				var type = typeof( IEnumerable<> ).MakeGenericType( dialog.TargetType );
				Root = InstanceFactory.Create( type );
				PropertyChanged.Raise( this, RootName );
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
