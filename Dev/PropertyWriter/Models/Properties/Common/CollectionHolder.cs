using System;
using System.Collections;
using System.Collections.ObjectModel;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;

namespace PropertyWriter.Models.Properties.Common
{
    internal class CollectionHolder
    {
        public static readonly string ElementTitle = "Element";

        private readonly PropertyFactory modelFactory_;
        public Type Type { get; }
        public Type ItemType { get; }
        public ObservableCollection<IPropertyModel> Collection { get; }
        public ReactiveProperty<IEnumerable> Value { get; }

        public CollectionHolder(Type type, PropertyFactory modelFactory)
        {
            Type = type;
            modelFactory_ = modelFactory;

            if (type.Name == "IEnumerable`1")
            {
                ItemType = type.GenericTypeArguments[0];
            }
            else if (type.IsArray)
            {
                ItemType = type.GetElementType();
            }
            else
            {
                throw new ArgumentException("配列または IEnumerable<T> を指定する必要があります。", nameof(type));
            }

            Value = new ReactiveProperty<IEnumerable>(Array.CreateInstance(ItemType, 0));

            Collection = new ObservableCollection<IPropertyModel>();
            Collection.ToCollectionChanged()
                .Subscribe(x => Value.Value = MakeValue(Collection));
        }

        private IEnumerable MakeValue(ObservableCollection<IPropertyModel> collection)
        {
            var array = Array.CreateInstance(ItemType, collection.Count);
            for (var i = 0; i < collection.Count; i++)
            {
                array.SetValue(collection[i].Value.Value, i);
            }
            return array;
        }

        public IPropertyModel AddNewElement()
        {
            var instance = modelFactory_.Create(ItemType, ElementTitle);
            Collection.Add(instance);
            instance.Value.Subscribe(x => Value.Value = MakeValue(Collection));
            return instance;
        }

        public void RemoveAt(int index)
        {
            Collection.RemoveAt(index);
        }
    }
}