using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	class BasicCollectionModel : PropertyModel
	{
        public BasicCollectionModel(Type type, ModelFactory modelFactory)
        {
			CollectionValue = new CollectionHolder(type, modelFactory);
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
