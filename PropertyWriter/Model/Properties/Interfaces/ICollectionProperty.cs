using PropertyWriter.Model.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model.Interfaces
{
    interface ICollectionProperty
    {
        IPropertyModel AddNewElement();
    }
}
