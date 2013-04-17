using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Helper methods for <see cref="IDisposable"/>.
	/// </summary>
	public static class DisposableHelper
	{
		/// <summary>
		/// Вызывает Dispose всех элементов коллекции.
		/// </summary>
		public static void DisposeAll([NotNull] this IEnumerable<IDisposable> disposables)
		{
			if (disposables == null)
				throw new ArgumentNullException("disposables");

			foreach (var disp in disposables)
				disp.Dispose();
		}

		/// <summary>
		/// Form grouped disposable from supplied elements.
		/// </summary>
		public static IDisposable Merge([NotNull] this IEnumerable<IDisposable> disposables)
		{
			if (disposables == null)
				throw new ArgumentNullException("disposables");
			var dispArray = disposables.ToArray();
			if (dispArray.Any(d => d == null))
				throw new ArgumentNullException();
			return new CompositeDisposable(dispArray);
		}
	}
}