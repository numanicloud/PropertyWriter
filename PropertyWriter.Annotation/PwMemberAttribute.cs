using System;

namespace PropertyWriter.Annotation
{
	[AttributeUsage(AttributeTargets.Property)]
	public class PwMasterAttribute : Attribute
	{
		public PwMasterAttribute(string name = null, string key = null)
		{
			Name = name;
			Key = key;
		}

		public string Name { get; }
		public string Key { get; }
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class PwMemberAttribute : Attribute
	{
		public PwMemberAttribute(string name = null)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class PwReferenceMemberAttribute : Attribute
	{
		public string Name { get; }
		public string MasterKey { get; }
		public string IdFieldName { get; }

		public PwReferenceMemberAttribute(string masterKey, string idFieldName, string name = null)
		{
			MasterKey = masterKey;
			IdFieldName = idFieldName;
			Name = name;
		}
	}
}