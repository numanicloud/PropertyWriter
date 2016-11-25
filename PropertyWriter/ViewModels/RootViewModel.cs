using System;
using PropertyWriter.Models;
using PropertyWriter.Models.Properties.Common;

namespace PropertyWriter.ViewModels
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
