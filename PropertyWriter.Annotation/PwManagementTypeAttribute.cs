using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PropertyWriter.Annotation
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
	public class PwProjectAttribute : Attribute
	{
	}

    [AttributeUsage(AttributeTargets.Method)]
    public class PwSerializerAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PwDeserializerAttribute : Attribute
    {
    }
}
