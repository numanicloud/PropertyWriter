using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model;
using PropertyWriter.Model.Instance;

namespace PropertyWriter.ViewModel
{
	class RootViewModel
	{
		public RootViewModel(Type type, MasterInfo[] masters)
		{
			Structure = new StructureHolder(type, masters);
		}

		public StructureHolder Structure { get; set; }
	}
}
