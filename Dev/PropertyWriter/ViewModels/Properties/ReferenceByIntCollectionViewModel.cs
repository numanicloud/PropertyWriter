using PropertyWriter.Models.Properties;
using PropertyWriter.ViewModels.Properties.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Subjects;
using Reactive.Bindings;

namespace PropertyWriter.ViewModels.Properties
{
	public class ReferenceByIntCollectionViewModel : CollectionViewModel<ReferenceByIntCollectionProperty>
	{
		public ReferenceByIntCollectionViewModel(ReferenceByIntCollectionProperty property, ViewModelFactory factory)
			: base(property, factory)
		{
		}
	}
}
