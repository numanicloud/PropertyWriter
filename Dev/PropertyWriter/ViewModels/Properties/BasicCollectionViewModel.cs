using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Livet.Messaging;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;
using System.Reactive;
using System.Reactive.Subjects;
using System.Diagnostics;

namespace PropertyWriter.ViewModels.Properties
{
    public class BasicCollectionViewModel : CollectionViewModel<BasicCollectionProperty>
	{
        public BasicCollectionViewModel(BasicCollectionProperty property, ViewModelFactory factory)
			: base(property, factory)
        {
		}
	}
}