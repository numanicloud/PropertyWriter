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
	class BasicCollectionModel : PropertyModel
	{
		public BasicCollectionModel(Type type)
		{
			CollectionValue = new CollectionHolder(type);
		}

		public ObservableCollection<IPropertyModel> Collection => CollectionValue.Collection;

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


		private CollectionHolder CollectionValue { get; set; }
	}
}
