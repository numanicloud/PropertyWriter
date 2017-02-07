using PropertyWriter.Models.Properties;
using PropertyWriter.ViewModels.Properties.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Subjects;

namespace PropertyWriter.ViewModels.Properties
{
	class SubtypingClassCollectionViewModel : ComplicateCollectionViewModel
	{
		public SubtypingClassCollectionViewModel(ComplicateCollectionProperty property, ViewModelFactory factory)
			: base(property, factory)
		{
		}
	}
}
