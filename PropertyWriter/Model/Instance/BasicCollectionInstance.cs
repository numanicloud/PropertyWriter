using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;
using System.Reflection;
using MvvmHelper;
using Reactive.Bindings;

namespace PropertyWriter.Model
{
	class BasicCollectionInstance : Instance
	{
		public BasicCollectionInstance(Type type)
		{
			CollectionValue = new CollectionValue(type);
		}

		public ObservableCollection<IInstance> Collection => CollectionValue.Collection;

		public override ReactiveProperty<object> Value => CollectionValue.Value
			.Cast<object>()
			.ToReactiveProperty();
		public override ReactiveProperty<string> FormatedString => Collection.ToCollectionChanged()
			.Select(x => "Count = " + Collection.Count)
			.ToReactiveProperty();


		public void AddNewProperty()
		{
			CollectionValue.AddNewProperty();
		}

		public void RemoveAt(int index)
		{
			CollectionValue.RemoveAt(index);
		}


		private CollectionValue CollectionValue { get; set; }
	}
}
