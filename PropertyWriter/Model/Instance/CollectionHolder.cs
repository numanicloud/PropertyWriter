using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	class CollectionHolder
	{
		public Type Type { get; }
		private readonly ModelFactory _modelFactory;

		public CollectionHolder(Type type, ModelFactory modelFactory)
		{
			Type = type;
			_modelFactory = modelFactory;

			if(type.Name == "IEnumerable`1")
			{
				this.ItemType = type.GenericTypeArguments[0];
			}
			else if(type.IsArray)
			{
				ItemType = type.GetElementType();
			}
			else
			{
				throw new ArgumentException("配列または IEnumerable<T> を指定する必要があります。", nameof(type));
			}

			Value = new ReactiveProperty<IEnumerable<object>>();
			Value.Value = Array.CreateInstance(ItemType, 0).Cast<object>();

			Collection = new ObservableCollection<IPropertyModel>();
			Collection.ToCollectionChanged()
				.Subscribe(x => Value.Value = MakeValue(Collection));
		}

		public ReactiveProperty<IEnumerable<object>> Value { get; }

		private IEnumerable<object> MakeValue(ObservableCollection<IPropertyModel> collection)
		{
			var array = Array.CreateInstance(ItemType, collection.Count);
			for(int i = 0; i < collection.Count; i++)
			{
				array.SetValue(collection[i].Value.Value, i);
			}
			return array.Cast<object>();
		}

		public ObservableCollection<IPropertyModel> Collection { get; }

		public void AddNewProperty()
		{
			var instance = _modelFactory.Create(ItemType);
			Collection.Add(instance);
		}

		public void RemoveAt(int index)
		{
			Collection.RemoveAt(index);
		}

		public Type ItemType { get; }
	}
}
