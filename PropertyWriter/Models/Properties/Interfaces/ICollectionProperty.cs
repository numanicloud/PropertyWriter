namespace PropertyWriter.Models.Properties.Interfaces
{
    interface ICollectionProperty : IPropertyModel
    {
        IPropertyModel AddNewElement();
    }
}
