using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;
using System.Diagnostics;
using PropertyWriter.Model.Properties;

namespace PropertyWriter.Model.Instance
{
	class ReferenceByIntViewModel : PropertyViewModel
	{
        private ReferenceByIntProperty Property { get; }

        public ReferencableMasterInfo Source => Property.Source;
		public ReactiveProperty<object> SelectedObject => Property.SelectedObject;
        public ReactiveProperty<int> IntValue => Property.IntValue;
        public override ReactiveProperty<object> Value => Property.Value;

        public ReferenceByIntViewModel(ReferenceByIntProperty property)
        {
            Property = property;
        }
	}
}
