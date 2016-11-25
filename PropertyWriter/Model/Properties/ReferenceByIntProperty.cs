using PropertyWriter.Model.Interfaces;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model.Properties
{
    class ReferenceByIntProperty : PropertyModel
    {
        public ReferencableMasterInfo Source { get; }
        public ReactiveProperty<object> SelectedObject { get; } = new ReactiveProperty<object>();
        public ReactiveProperty<int> IntValue { get; }
        public override ReactiveProperty<object> Value { get; }
        public PropertyInfo PropertyToReference { get; }
        public PropertyInfo PropertyToBindBack { get; set; }

        public ReferenceByIntProperty(ReferencableMasterInfo source, string idPropertyName)
        {
            Source = source;
            PropertyToReference = Source.Type.GetProperty(idPropertyName);
            IntValue = SelectedObject.Where(x => x != null)
                .Select(x => (int)(PropertyToReference?.GetValue(x) ?? -1))
                .ToReactiveProperty(-1);
            Value = IntValue.Select(x => (object)x).ToReactiveProperty();
        }

        public void SetItemById(int id)
        {
            if (PropertyToReference == null)
            {
                throw new InvalidOperationException();
            }

            var obj = Source.Collection.FirstOrDefault(x => (int)PropertyToReference.GetValue(x) == id);
            if (obj != null)
            {
                SelectedObject.Value = obj;
            }
        }
    }
}
