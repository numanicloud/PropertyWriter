using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Livet.Messaging;
using PropertyWriter.Models.Properties;
using PropertyWriter.Models.Properties.Interfaces;
using PropertyWriter.ViewModels.Properties.Common;
using Reactive.Bindings;
using System.Diagnostics;

namespace PropertyWriter.ViewModels.Properties
{
    public class ClassViewModel : StructureHolderViewModel<ClassProperty>
	{
		public ClassViewModel(ClassProperty property, ViewModelFactory factory)
			: base(property, factory)
		{
			string GetString()
			{
				return Value?.Value?.ToString();
			}

			FormatedString.Dispose();
			FormatedString = Property.OnChanged
				.Select(x => GetString())
				.ToReactiveProperty(GetString());
        }
	}
}
