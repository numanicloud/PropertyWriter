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
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class PwMasterAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class PwMinorAttribute : Attribute
	{
	}
}
