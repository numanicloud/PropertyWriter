using System;

namespace PropertyWriter.Models.Properties.Interfaces
{
    interface IStructureProperty
    {
        Type ValueType { get; }
        IPropertyModel[] Members { get; }
    }
}
