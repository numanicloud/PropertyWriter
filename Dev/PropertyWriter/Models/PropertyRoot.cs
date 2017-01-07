using System;
using PropertyWriter.Models.Info;
using PropertyWriter.Models.Properties.Common;
using PropertyWriter.ViewModels;
using System.Diagnostics;

namespace PropertyWriter.Models
{
    class PropertyRoot
    {
        public PropertyRoot(Type type, MasterInfo[] masters)
        {
            Type = type;
            Structure = new StructureHolder(type, masters);
			OnError = Structure.OnError;
			OnError.Subscribe(x => Debugger.Log(1, "Error", "Error from Root\n"));
        }

        public Type Type { get; }
        public StructureHolder Structure { get; }
		public IObservable<Exception> OnError { get; }
    }
}
