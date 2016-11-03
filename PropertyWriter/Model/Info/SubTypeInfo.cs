using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model.Info
{
	class SubTypeInfo
	{
		public SubTypeInfo(Type type, string name)
		{
			Type = type;
			Name = name;
		}

		public string Name { get; private set; }
		public Type Type { get; private set; }
	}
}
