using System;

namespace PropertyWriter.Models.Properties.Interfaces
{
    interface IStructureProperty
    {
        Type Type { get; }
        IPropertyModel[] Members { get; }
    }
}
