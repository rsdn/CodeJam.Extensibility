using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Строковые утилиты.
	/// </summary>
	public static class StringUtils
	{
		/// <summary>
		/// Инфиксная форма <see cref="string.IsNullOrEmpty"/>
		/// </summary>
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		/// <summary>
		/// <see cref="string.Format(string,object[])"/> в инфиксной форме.
		/// </summary>
		[StringFormatMethod("formatString")]
		public static string FormatStr(this string formatString, params object[] args)
		{
			return string.Format(formatString, args);
		}

		/// <summary>
		/// Format each element of enumerable. Format string can use {0} placeholder for current source item.
		/// </summary>
		public static IEnumerable<string> FormatStr(this IEnumerable<string> source, string formatStr)
		{
			return source.Select(str => formatStr.FormatStr(str));
		}

		/// <summary>
		/// <see cref="string.Format(IFormatProvider,string,object[])"/> в инфиксной форме.
		/// </summary>
		public static string FormatStr(
			this string formatString,
			IFormatProvider format,
			params object[] args)
		{
			return string.Format(format, formatString, args);
		}

		/// <summary>
		/// Аналог <see cref="string.Join(string,string[])"/>, но
		/// работающий с перечислениями.
		/// </summary>
		public static string Join(this IEnumerable<string> source)
		{
			return Join(source, null);
		}

		/// <summary>
		/// Аналог <see cref="string.Join(string,string[])"/>, но
		/// работающий с перечислениями.
		/// </summary>
		public static string Join(this IEnumerable<string> source, string separator)
		{
			var sb = new StringBuilder();
			foreach (var str in source)
			{
				if (separator != null && sb.Length > 0)
					sb.Append(separator);
				sb.Append(str);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Разделить строку по разделителю с отрезанием пробельных символов.
		/// </summary>
		public static IEnumerable<string> SplitWithTrim(this string source, params char[] separators)
		{
			return source.Split(separators).Select(part => part.Trim());
		}
	}
}
