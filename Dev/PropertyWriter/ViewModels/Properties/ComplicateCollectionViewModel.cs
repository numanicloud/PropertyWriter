using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using Livet.Messaging;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;
using System.Reactive.Subjects;
using System.Collections.Generic;

namespace PropertyWriter.ViewModels.Properties
{
	public class ComplicateCollectionViewModel : CollectionViewModel<ComplicateCollectionProperty>
	{
		public ComplicateCollectionViewModel(ComplicateCollectionProperty property, ViewModelFactory factory)
			: base(property, factory)
		{
		}
	}
}