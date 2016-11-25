using PropertyWriter.Model.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model
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
