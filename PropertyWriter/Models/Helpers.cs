using System;
using System.Linq;
using System.Reflection;

namespace PropertyWriter.Models
{
    static class Helpers
    {
        public static bool IsAnnotatedType<TAttribute>(Type type)
        {
            return type.CustomAttributes.Any(x => x.AttributeType == typeof(TAttribute));
        }

        public static bool IsAnnotatedMember<TAttribute>(MemberInfo member)
        {
            return member.CustomAttributes.Any(x => x.AttributeType == typeof(TAttribute));
        }
    }
}
