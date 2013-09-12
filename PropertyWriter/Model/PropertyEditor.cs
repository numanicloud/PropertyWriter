using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace PropertyWriter.Model
{
	class PropertyEditor
	{
		public bool IsInitialized { get; private set; }
		public Type Type { get; private set; }
		public PropertyInfo[] Info { get; private set; }
		public IEnumerable<TargetOfEdit> Data
		{
			get { return data_; }
		}

		public List<TargetOfEdit> data_ { get; private set; }

		public void Initialize( System.Type type )
		{
			this.Type = type;
			data_ = new List<TargetOfEdit>();
			Info = type.GetProperties()
				.Where( _ => _.GetCustomAttributes( typeof( DataMemberAttribute ) ).Any() )
				.ToArray();
			IsInitialized = true;
		}

		public TargetOfEdit CreateData()
		{
			return new TargetOfEdit(Info);
		}

		public void AddNewData( TargetOfEdit data )
		{
			data_.Add( data );
		}

		public void RemoveData( int index )
		{
			data_.RemoveAt( index );
		}
	}
}
