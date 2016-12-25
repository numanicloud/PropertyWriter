using System;
using System.Diagnostics;
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

		public static void SafelySubscribe<T>(this IObservable<T> source, Action<Exception> onError)
		{
			source.Subscribe(
				unit => { },
				exception =>
				{
					onError(exception);
					Debugger.Log(1, "Error", exception + "\n");
					SafelySubscribe(source, onError);
				});
		}
	}
}
