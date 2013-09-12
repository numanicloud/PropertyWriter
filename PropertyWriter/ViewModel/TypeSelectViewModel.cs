using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;

namespace PropertyWriter
{
	class TypeSelectViewModel : INotifyPropertyChanged
	{
		public TypeSelectViewModel()
		{
			OpenCommand = new DelegateCommand
			{
				ExecuteHandler = o => LoadDll(),
			};
			DllName = "No DLL";
			Types = new Type[0];
			Properties = new PropertyInfo[0];
			selectedTypeIndex_ = 0;
		}

		public DelegateCommand OpenCommand { get; private set; }
		public string DllName { get; private set; }
		public Type[] Types { get; private set; }
		public PropertyInfo[] Properties { get; private set; }
		public int SelectedTypeIndex
		{
			get { return selectedTypeIndex_; }
			set
			{
				selectedTypeIndex_ = value;
				if( value >= 0 )
				{
					Properties = DllLoader.LoadProperties( Types[value] );
					Raise( "Properties" );
				}
			}
		}
		private int selectedTypeIndex_;

		private void LoadDll()
		{
			var dialog = new OpenFileDialog
			{
				FileName = "",
				Filter = "アセンブリ ファイル (*.dll, *.exe)|*.dll;*.exe",
				Title = "アセンブリを開く"
			};
			if( dialog.ShowDialog() == true )
			{
				DllName = Path.GetFileName( dialog.FileName );
				Raise( "DllName" );
				Types = DllLoader.LoadDataTypes( dialog.FileName );
				Raise( "Types" );
				Properties = new PropertyInfo[0];
				Raise( "Properties" );
				SelectedTypeIndex = -1;
				Raise( "SelectedTypeIndex" );
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		public void Raise( string propertyName )
		{
			var ev = PropertyChanged;
			if( ev != null )
			{
				ev( this, new PropertyChangedEventArgs( propertyName ) );
			}
		}
	}
}
