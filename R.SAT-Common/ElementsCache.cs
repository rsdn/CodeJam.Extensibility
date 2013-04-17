#pragma warning disable 1692
using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Делегат для создания элемента кеша.
	/// </summary>
	public delegate TElement CreateElement<TKey, TElement>(TKey key);

	/// <summary>
	/// Кеш элементов. Для создания необходим параметр.
	/// </summary>
	public class ElementsCache<TKey, TElement>
	{
		private readonly CreateElement<TKey, TElement> _elementCreator;
		private readonly Dictionary<TKey, TElement> _elements;
		private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();

		/// <summary>
		/// Инициализирует экземпляр с указанием компаратора ключей.
		/// </summary>
		public ElementsCache(
			[NotNull] CreateElement<TKey, TElement> elementCreator,
			[CanBeNull] IEqualityComparer<TKey> comparer)
		{
			if (elementCreator == null) throw new ArgumentNullException("elementCreator");
			_elements = new Dictionary<TKey, TElement>(comparer);
			_elementCreator = elementCreator;
		}

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public ElementsCache([NotNull] CreateElement<TKey, TElement> elementCreator)
			: this(elementCreator, null)
		{}

		/// <summary>
		/// Создатель элементов.
		/// </summary>
		[NotNull]
		public CreateElement<TKey, TElement> ElementCreator
		{
			get { return _elementCreator; }
		}

		/// <summary>
		/// Коллекция всех ключей.
		/// </summary>
		[NotNull]
		public ICollection<TKey> Keys
		{
			get
			{
				using (_cacheLock.GetReaderLock())
				{
					var keys = new List<TKey>(_elements.Keys);
					return keys;
				}
			}
		}

		/// <summary>
		/// Извлекает элемент из кеша.
		/// </summary>
		public TElement Get([NotNull] TKey key)
		{
#pragma warning disable CompareNonConstrainedGenericWithNull
			if (key == null)
#pragma warning restore CompareNonConstrainedGenericWithNull
				throw new ArgumentNullException("key");
			using (_cacheLock.GetUpgradeableReaderLock())
			{
				TElement element;
				if (!_elements.TryGetValue(key, out element))
					using (_cacheLock.GetWriterLock())
						if (!_elements.TryGetValue(key, out element))
						{
							element = _elementCreator(key);
							_elements.Add(key, element);
							OnAfterElementCreated(key, element);
						}
				return element;
			}
		}

		/// <summary>
		/// Вызывается после создания нового элемента.
		/// </summary>
		protected virtual void OnAfterElementCreated(TKey key, TElement element)
		{
		}

		/// <summary>
		/// Производит попытку извлечения элемента.
		/// </summary>
		public bool TryGet([NotNull] TKey key, out TElement element)
		{
#pragma warning disable CompareNonConstrainedGenericWithNull
			if (key == null)
#pragma warning restore CompareNonConstrainedGenericWithNull
				throw new ArgumentNullException("key");
			using (_cacheLock.GetReaderLock())
				return _elements.TryGetValue(key, out element);
		}

		/// <summary>
		/// Возвращает, содержит ли кеш элемент с указанным ключем.
		/// </summary>
		public bool Contains([NotNull] TKey key)
		{
#pragma warning disable CompareNonConstrainedGenericWithNull
			if (key == null)
#pragma warning restore CompareNonConstrainedGenericWithNull
				throw new ArgumentNullException("key");
			using (_cacheLock.GetReaderLock())
				return _elements.ContainsKey(key);
		}

		/// <summary>
		/// Удаляет элемент кеша по заданному ключу.
		/// </summary>
		public virtual void Drop([NotNull] TKey key)
		{
#pragma warning disable CompareNonConstrainedGenericWithNull
			if (key == null)
#pragma warning restore CompareNonConstrainedGenericWithNull
				throw new ArgumentNullException("key");
			using (_cacheLock.GetWriterLock())
				_elements.Remove(key);
		}

		/// <summary>
		/// Сбросить кеш.
		/// </summary>
		public virtual void Reset()
		{
			using (_cacheLock.GetWriterLock())
				_elements.Clear();
		}
	}
}