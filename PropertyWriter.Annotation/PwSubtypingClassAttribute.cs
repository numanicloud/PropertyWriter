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
		public string Name { get; private set; }

		public PwSubtypeAttribute(string name = null)
		{
			Name = name;
		}
	}
}