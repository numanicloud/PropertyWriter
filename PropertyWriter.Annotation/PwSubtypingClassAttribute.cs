using System;

namespace PropertyWriter.Annotation
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class PwSubtypingAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class PwSubtypeAttribute : Attribute
	{
	}
}