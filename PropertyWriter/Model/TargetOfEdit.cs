using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model
{
	class TargetOfEdit
	{
		public TargetOfEdit( PropertyInfo[] properties )
		{
			this.Properties = properties
				.Select( _ => new PropertyToWrite( _ ) )
				.ToArray();
		}

		public PropertyToWrite[] Properties { get; private set; }
	}
}
