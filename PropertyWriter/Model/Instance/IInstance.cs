using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Model
{
	interface IInstance : INotifyPropertyChanged
	{
		object Value { get; }
	}
}
