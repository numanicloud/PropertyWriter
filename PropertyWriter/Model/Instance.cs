using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;

namespace PropertyWriter.Model
{
	abstract class Instance : IInstance
	{
		public abstract object Value { get; }
		public virtual string FormatedString
		{
			get { return Value.ToString(); }
		}

		public abstract event PropertyChangedEventHandler PropertyChanged;
		internal static readonly string FormatedStringName = PropertyName<Instance>.Get( _ => _.FormatedString );
	}
}
