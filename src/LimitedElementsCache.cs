using System;
using System.Collections.Generic;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Кеш элементов с ограничением максимального размера.
	/// </summary>
	public class LimitedElementsCache<TKey, TElement> : ElementsCache<TKey, TElement>
	{
		private readonly Queue<TKey> _queue = new Queue<TKey>();

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public LimitedElementsCache(CreateElement<TKey, TElement> elementCreator, int maxSize)
			: base(elementCreator)
		{
			MaxSize = maxSize;
		}

		/// <summary>
		/// Максимальный размер кеша.
		/// </summary>
		public int MaxSize { get; }

		/// <summary>
		/// Вызывается после создания нового элемента.
		/// </summary>
		protected override void OnAfterElementCreated(TKey key, TElement element)
		{
			base.OnAfterElementCreated(key, element);
			_queue.Enqueue(key);
			if (_queue.Count > MaxSize)
				base.Drop(_queue.Dequeue());
		}

		/// <summary>
		/// Удаляет элемент кеша по заданному ключу.
		/// </summary>
		public override void Drop(TKey key)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Сбросить кеш.
		/// </summary>
		public override void Reset()
		{
			base.Reset();
			_queue.Clear();
		}
	}
}