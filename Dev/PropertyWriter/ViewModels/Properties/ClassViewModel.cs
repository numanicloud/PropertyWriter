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
			FormatedString.Dispose();
			FormatedString = Property.OnChanged
				.Select(x => Value.Value.ToString())
				.Do(x => { }, ex => Debugger.Log(1, "Error", $"Error from ClassViewModel:\n{ex}\n"))
				.ToReactiveProperty(Value.Value.ToString());
        }
	}
}
