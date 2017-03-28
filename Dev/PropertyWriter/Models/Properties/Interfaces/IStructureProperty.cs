using System;

namespace PropertyWriter.Models.Properties.Interfaces
{
    public interface IStructureProperty
    {
        Type ValueType { get; }
        IPropertyModel[] Members { get; }
    }
}
