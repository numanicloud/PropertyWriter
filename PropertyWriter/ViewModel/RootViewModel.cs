using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyWriter.Model;
using PropertyWriter.Model.Properties;

namespace PropertyWriter.ViewModel
{
	class RootViewModel
	{
        private PropertyRoot Root { get; }

        public Type Type => Root.Type;
        public StructureHolder Structure => Root.Structure;

        public RootViewModel(PropertyRoot root)
		{
            Root = root;
		}
	}
}
