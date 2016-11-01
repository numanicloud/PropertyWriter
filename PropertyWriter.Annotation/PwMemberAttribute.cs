using System;

namespace PropertyWriter.Annotation
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class PwMemberAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class PwReferenceMemberAttribute : Attribute
	{
		public Type TargetType { get; }
		public string IdFieldName { get; }

		public PwReferenceMemberAttribute(Type targetType, string idFieldName)
		{
			TargetType = targetType;
			this.IdFieldName = idFieldName;
		}
	}
}