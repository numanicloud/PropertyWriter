using System;

namespace PropertyWriter.Models.Info
{
	class SubTypeInfo
	{
		public SubTypeInfo(Type type, string name)
		{
			Type = type;
			Name = name ?? type.Name;
		}

		public string Name { get; private set; }
		public Type Type { get; private set; }
	}
}
