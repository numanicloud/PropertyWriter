using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.ViewModels
{
	static class ObservableHelper
	{
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
