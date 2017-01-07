using System;
using System.Collections;
using System.Collections.ObjectModel;
using PropertyWriter.Models.Properties.Interfaces;
using Reactive.Bindings;
using System.Reactive.Subjects;

namespace PropertyWriter.Models.Properties.Common
{
	internal class CollectionHolder
	{
		public static readonly string ElementTitle = "Element";

		private readonly PropertyFactory modelFactory_;

		private Subject<Exception> OnErrorSubject { get; } = new Subject<Exception>();

		public Type Type { get; }
		public Type ItemType { get; }
		public ObservableCollection<(IPropertyModel model, IDisposable error)> Collection { get; }
		public ReactiveProperty<IEnumerable> Value { get; }
		public IObservable<Exception> OnError => OnErrorSubject;

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

			Collection = new ObservableCollection<(IPropertyModel model, IDisposable error)>();
			Collection.ToCollectionChanged()
				.Subscribe(x =>
				{
					try
					{
						Value.Value = MakeValue(Collection);
					}
					catch (Exception e)
					{
						OnErrorSubject.OnNext(e);
						throw;
					}
				});
		}

		private IEnumerable MakeValue(ObservableCollection<(IPropertyModel model, IDisposable error)> collection)
		{
			var array = Array.CreateInstance(ItemType, collection.Count);
			for (var i = 0; i < collection.Count; i++)
			{
				array.SetValue(collection[i].model.Value.Value, i);
			}
			return array;
		}

		public IPropertyModel AddNewElement()
		{
			var instance = modelFactory_.Create(ItemType, ElementTitle);
			var errorDisposable = instance.OnError.Subscribe(x => OnErrorSubject.OnNext(x));
			Collection.Add((instance, errorDisposable));
			instance.Value.Subscribe(x => Value.Value = MakeValue(Collection));
			return instance;
		}

		public void RemoveAt(int index)
		{
			Collection[index].error.Dispose();
			Collection.RemoveAt(index);
		}
	}
}