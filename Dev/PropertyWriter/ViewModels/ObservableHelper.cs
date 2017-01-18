using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
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

		public static IDisposable PublishTask<T>(this IObservable<T> source, Func<T, Task> selector, Action<Exception> onError)
		{
			return source.SelectMany(x => selector(x).ToObservable())
				.Catch((Exception err) =>
				{
					onError(err);
					return Observable.Empty<Unit>();
				}).Repeat()
				.Subscribe();
		}
	}
}
