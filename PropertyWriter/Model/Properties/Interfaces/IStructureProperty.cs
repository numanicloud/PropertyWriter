using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model.Properties
{
    interface IStructureProperty
    {
        Type Type { get; }
        IPropertyModel[] Members { get; }
    }
}
