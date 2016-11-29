using System;
using PropertyWriter.Models.Info;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.ViewModels;

namespace PropertyWriter.Models
{
    class PropertyRoot
    {
        public PropertyRoot(Type type, MasterInfo[] masters)
        {
            Type = type;
            Structure = new StructureHolder(type, masters);
        }

        public Type Type { get; }
        public StructureHolder Structure { get; }
    }
}
