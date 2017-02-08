using Reactive.Bindings;

namespace PropertyWriter.Models.Properties.Interfaces
{
    public interface ICollectionProperty : IPropertyModel
    {
		ReadOnlyReactiveCollection<IPropertyModel> Collection { get; }
        IPropertyModel AddNewElement();
		void RemoveElementAt(int index);
		void Move(int oldIndex, int newIndex);
    }
}
