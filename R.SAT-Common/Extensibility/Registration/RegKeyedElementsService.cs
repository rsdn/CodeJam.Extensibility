using System;
using System.Collections.Generic;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Базовая реализация <see cref="IRegKeyedElementsService{K, EI}"/>
	/// </summary>
	public class RegKeyedElementsService<TKey, TInfo> : RegElementsService<TInfo>,
		IRegKeyedElementsService<TKey, TInfo>
			where TInfo : class, IKeyedElementInfo<TKey>
			where TKey : class
	{
		private readonly Dictionary<TKey, TInfo> _byNameMap =
			new Dictionary<TKey, TInfo>();

		#region IRegKeyedElementsService<K,EI> Members
		/// <summary>
		/// Зарегистрировать элемент.
		/// </summary>
		public override void Register(TInfo elementInfo)
		{
			lock (_byNameMap)
			{
				if (_byNameMap.ContainsKey(elementInfo.Key))
					throw new ArgumentException("Element with name '" + elementInfo.Key
						+ "' already registered");
				base.Register(elementInfo);
				_byNameMap.Add(elementInfo.Key, elementInfo);
			}
		}

		/// <summary>
		/// Проверяет, есть ли зарегистрированный элемент с указанным именем.
		/// </summary>
		public bool ContainsElement(TKey key)
		{
			if (key == null)
				throw new ArgumentNullException("key");
			return _byNameMap.ContainsKey(key);
		}

		/// <summary>
		/// Возвращает элемент по его имени.
		/// </summary>
		public TInfo GetElement(TKey key)
		{
			if (key == null)
				throw new ArgumentNullException("key");
			TInfo elementInfo;
			if (_byNameMap.TryGetValue(key, out elementInfo))
				return elementInfo;
			throw new ArgumentException("Element with key '" + key
				+ "' is not registered");
		}
		#endregion
	}
}