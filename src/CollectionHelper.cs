using System;
using System.Collections.Generic;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Утилиты для работы с коллекциями.
	/// </summary>
	public static class CollectionHelper
	{
		#region Delegates
		/// <summary>
		/// Делегат, описывающий метод преобразования коллекции к словарю.
		/// </summary>
		public delegate TKey GetKey<out TKey, in TValue>(TValue source);

		/// <summary>
		/// Селектор.
		/// </summary>
		public delegate T2 Selector<in T1, out T2>(T1 source);
		#endregion

		/// <summary>
		/// Преобразует входную коллекцию в выходной массив при помощи переданного делегата.
		/// </summary>
		public static TOut[] ConvertAll<TIn, TOut>(this ICollection<TIn> collection,
			Converter<TIn, TOut> converter)
		{
			var res = new TOut[collection.Count];
			var i = 0;
			foreach (var elem in collection)
			{
				res[i] = converter(elem);
				i++;
			}
			return res;
		}

		/// <summary>
		/// Возвращает реверсивный вариант сравнения.
		/// </summary>
		public static Comparison<T> ReverseComparision<T>(Comparison<T> srcComparision)
		{
			return
				(e1, e2) =>
					{
						var cr = srcComparision(e1, e2);
						return cr > 0 ? -1 : cr < 0 ? 1 : 0;
					};
		}
	}
}