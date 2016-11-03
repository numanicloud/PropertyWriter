using System;

namespace PropertyWriter.Annotation
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class PwMemberAttribute : Attribute
	{
		public PwMemberAttribute(string name = null)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class PwReferenceMemberAttribute : Attribute
	{
		public string Name { get; }
		public Type TargetType { get; }
		public string IdFieldName { get; }

		public PwReferenceMemberAttribute(Type targetType, string idFieldName, string name = null)
		{
			TargetType = targetType;
			this.IdFieldName = idFieldName;
			Name = name;
		}
	}
}