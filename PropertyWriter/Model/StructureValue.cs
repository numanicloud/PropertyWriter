using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropertyWriter.Model
{
	class StructureValue
	{
		public StructureValue( Type type )
		{
			Properties = DllLoader.LoadProperties( type )
				.Select( _ => new PropertyInstance( _ ) )
				.ToArray();
			Properties.ForEach( _ => _.OnValueChanged += __OnValueChanged );
			this.Value = Activator.CreateInstance( type );
		}

		public IEnumerable<IInstance> Instances
		{
			get { return Properties.Select( _ => _.Instance ).ToArray(); }
		}
		public IEnumerable<PropertyInstance> Properties { get; private set; }
		public object Value { get; private set; }

		public event Action OnValueChanged;


		void __OnValueChanged( PropertyInstance instance )
		{
			var value = InstanceConverter.Convert( instance.Instance.Value, instance.PropertyInfo.PropertyType );
			instance.PropertyInfo.SetValue( Value, value );
			OnValueChanged();
		}
	}
}
