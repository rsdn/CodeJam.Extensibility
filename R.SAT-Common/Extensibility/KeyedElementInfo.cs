using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Default <see cref="IKeyedElementInfo{TKey}"/> implementation.
	/// </summary>
	public abstract class KeyedElementInfo<TKey> : ElementInfo, IKeyedElementInfo<TKey>
	{
		private readonly TKey _key;

		/// <summary>
		/// Initialize instance.
		/// </summary>
		protected KeyedElementInfo([NotNull] Type type, TKey key)
			: base(type)
		{
			_key = key;
		}

		/// <summary>
		/// Initialize instance.
		/// </summary>
		protected KeyedElementInfo([NotNull] Type type, TKey key, string description)
			: base(type, description)
		{
			_key = key;
		}

		/// <summary>
		/// Key.
		/// </summary>
		public TKey Key
		{
			get { return _key; }
		}
	}
}