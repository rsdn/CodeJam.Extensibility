using System;

using CodeJam.Extensibility.Registration;

using JetBrains.Annotations;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Default <see cref="IKeyedElementInfo{TKey}"/> implementation.
	/// </summary>
	public abstract class KeyedElementInfo<TKey> : ElementInfo, IKeyedElementInfo<TKey>
	{
		/// <summary>
		/// Initialize instance.
		/// </summary>
		protected KeyedElementInfo([NotNull] Type type, TKey key)
			: base(type)
		{
			Key = key;
		}

		/// <summary>
		/// Initialize instance.
		/// </summary>
		protected KeyedElementInfo([NotNull] Type type, TKey key, string description)
			: base(type, description)
		{
			Key = key;
		}

		/// <summary>
		/// Key.
		/// </summary>
		public TKey Key { get; }
	}
}