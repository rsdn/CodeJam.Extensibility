using System.Collections.Generic;
using System.Linq;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Строковые утилиты.
	/// </summary>
	public static class StringUtils
	{
		/// <summary>
		/// Разделить строку по разделителю с отрезанием пробельных символов.
		/// </summary>
		public static IEnumerable<string> SplitWithTrim(this string source, params char[] separators)
		{
			return source.Split(separators).Select(part => part.Trim());
		}
	}
}
