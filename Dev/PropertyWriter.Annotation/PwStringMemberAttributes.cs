using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.Annotation
{
	[AttributeUsage(AttributeTargets.Property)]
	public class PwMultiLineTextMemberAttribute : Attribute
	{
		public PwMultiLineTextMemberAttribute(string name = null)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}
}
