using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PropertyWriter.Annotation
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class PwGlobalAttribute : Attribute
	{
		public PwGlobalAttribute(string name = null)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class PwMasterAttribute : Attribute
	{
		public PwMasterAttribute(string name = null)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class PwMinorAttribute : Attribute
	{
	}
}
