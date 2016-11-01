using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace PropertyWriter.Model.Instance
{
	class ReferenceByIntModel : PropertyModel
	{
		private readonly ComplicateCollectionModel source_;
		private readonly InstanceAndMemberInfo memberInfo_;

		public ReadOnlyReactiveCollection<object> Source => source_.Collection.ToReadOnlyReactiveCollection(x => x.Value.Value);
		public ReactiveProperty<object> SelectedObject { get; } = new ReactiveProperty<object>(); 
		public ReactiveProperty<int> IntValue => SelectedObject.Select(x => (int)(memberInfo_?.GetValue(x) ?? -1))
			.ToReactiveProperty();
		public override ReactiveProperty<object> Value => IntValue.Select(x => (object)x).ToReactiveProperty();

		public ReferenceByIntModel(ComplicateCollectionModel source, string idFieldName)
		{
			source_ = source;

			var property = source.ElementType.GetProperty(idFieldName);
			if (property != null)
			{
				memberInfo_ = InstanceAndPropertyInfo.ForMember(property);
			}

			var field = source.ElementType.GetField(idFieldName);
			if (field != null)
			{
				memberInfo_ = InstanceAndFieldInfo.ForMember(field);
			}
		}
	}
}
