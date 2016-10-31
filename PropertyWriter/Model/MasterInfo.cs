using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model
{
	class MasterInfo
	{
		public string TypeName { get; private set; }
		public IPropertyModel Master { get; private set; }

		public MasterInfo(string typeName, IPropertyModel master)
		{
			TypeName = typeName;
			Master = master;
		}
	}
}
