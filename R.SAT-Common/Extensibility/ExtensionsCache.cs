using System;
using System.Collections.Generic;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Keyed extension instances cache.
	/// Thread safe.
	/// </summary>
	public class ExtensionsCache<TInfo, TKey, TElement>
	{
		public IEnumerable<TElement> GetAllElements()
		{
			throw new NotImplementedException();
		}

		public TElement GetElement(TKey key)
		{
			throw new NotImplementedException();
		}
	}
}