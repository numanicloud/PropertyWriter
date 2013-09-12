using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model;
using MvvmHelper;

namespace PropertyWriter
{
	class EditorViewModel : INotifyPropertyChanged
	{
		public EditorViewModel()
		{
			Editor = new PropertyEditor();

			NewFileCommand = new DelegateCommand
			{
				ExecuteHandler = OnNewFile,
			};
			AddCommand = new DelegateCommand
			{
				ExecuteHandler = o => AddData(),
				CanExecuteHandler = o => Editor.IsInitialized,
			};
			RemoveCommand = new DelegateCommand
			{
				ExecuteHandler = RemoveData,
				CanExecuteHandler = o => Editor.IsInitialized,
			};
		}

		public int DataIndex
		{
			get { return dataIndex_; }
			set
			{
				dataIndex_ = value;
				PropertyChanged.Raise( this, CurrentPropertiesName );
			}
		}
		public IEnumerable<PropertyToWrite> CurrentProperties
		{
			get
			{
				if( Editor.IsInitialized )
				{
					return DataIndex >= 0 && DataIndex < Editor.Data.Count() ? Editor.Data.ElementAt( DataIndex ).Properties : null;
				}
				return null;
			}
		}
		public int DataNameIndex
		{
			get { return dataNameIndex_; }
			set
			{
				dataNameIndex_ = value;
				PropertyChanged.Raise( this, DataNamesName );
			}
		}
		public string[] IndexToName
		{
			get
			{
				if( Editor.IsInitialized )
				{
					return Editor.Info
						.Select( _ => _.Name )
						.ToArray();
				}
				return null;
			}
		}
		public IEnumerable<string> DataNames
		{
			get
			{
				if( !Editor.IsInitialized )
				{
					return null;
				}
				if( DataNameIndex < 0 )
				{
					return Enumerable.Range( 0, Editor.Data.Count() )
						.Select( _ => "Data " + _ )
						.ToArray();
				}
				else
				{
					return Editor.Data
						.Select( _ => _.Properties.First( _2 => _2.Info.Name == IndexToName[DataNameIndex] ) )
						.Select( _ => _.Value.ToString() )
						.ToArray();
				}
			}
		}
		public DelegateCommand AddCommand { get; private set; }
		public DelegateCommand RemoveCommand { get; private set; }
		public DelegateCommand NewFileCommand { get; private set; }
		public DelegateCommand OpenCommand { get; private set; }

		private PropertyEditor Editor { get; set; }
		private int dataIndex_;
		private int dataNameIndex_;

		private void OnNewFile( object obj )
		{
			var dialog = new TypeSelectWindow();
			dialog.ShowDialog();
			if( dialog.TargetType != null )
			{
				Editor.Initialize( dialog.TargetType );
				AddCommand.RaiseCanExecuteChanged();
				PropertyChanged.Raise( this, IndexToNameName );

				DataIndex = -1;
				DataNameIndex = -1;
				PropertyChanged.Raise( this, DataIndexName );
				PropertyChanged.Raise( this, DataNameIndexName );
			}
		}

		private void AddData()
		{
			var data = Editor.CreateData();
			Editor.AddNewData( data );

			data.Properties.ForEach( _ => _.PropertyChanged += ( o, sender ) =>
				PropertyChanged.Raise( this, CurrentPropertiesName ) );
			PropertyChanged.Raise( this, DataNamesName );
		}

		private void RemoveData( object obj )
		{
			Editor.RemoveData( DataIndex );
			DataIndex = -1;
			PropertyChanged.Raise( this, DataNamesName );
		}



		public event PropertyChangedEventHandler PropertyChanged;
		internal static string DataNamesName = MvvmHelper.PropertyName<EditorViewModel>.Get( _ => _.DataNames );
		internal static string CurrentPropertiesName = PropertyName<EditorViewModel>.Get( _ => _.CurrentProperties );
		internal static string IndexToNameName = PropertyName<EditorViewModel>.Get( _ => _.IndexToName );
		internal static string DataIndexName = PropertyName<EditorViewModel>.Get( _ => _.DataIndex );
		internal static string DataNameIndexName = PropertyName<EditorViewModel>.Get( _ => _.DataNameIndex );
	}
}
