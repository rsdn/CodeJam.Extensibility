using System;

using JetBrains.Annotations;

using System.Linq;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Keyed extension instances cache.
	/// Thread safe.
	/// </summary>
	public class ExtensionsCache<TInfo, TKey, TElement>
		where TInfo : KeyedElementInfo<TKey>
		where TElement : class
	{
		private readonly InstancingCustomParam[] _customParams;
		private readonly IServiceProvider _provider;
		private readonly ElementsCache<TKey, TElement> _cache;

		/// <summary>
		/// Initialize instance.
		/// </summary>
		public ExtensionsCache(
			[NotNull] IServiceProvider provider,
			[CanBeNull] Func<TInfo, TElement> instanceCreator)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");
			_provider = provider;
			_customParams = EmptyArray<InstancingCustomParam>.Value;
			_cache =
				new ElementsCache<TKey, TElement>(
					instanceCreator == null
						? (CreateElement<TKey, TElement>)CreateByHelper
						: key => CreateByCreator(instanceCreator, key));
		}

		/// <summary>
		/// Initialize instance.
		/// </summary>
		public ExtensionsCache(
				[NotNull] IServiceProvider provider,
				params InstancingCustomParam[] customParams)
			: this (provider, (Func<TInfo, TElement>)null)
		{
			_customParams = customParams;
		}

		private TElement CreateByCreator(Func<TInfo, TElement> instanceCreator, TKey key)
		{
			return instanceCreator(GetExtensionInfo(key));
				
		}

		private TElement CreateByHelper(TKey key)
		{
			var info = GetExtensionInfo(key);
			return
				info != null
					? (TElement)
						info
							.Type
							.CreateInstance(_provider, _customParams)
					: null;
		}

		/// <summary>
		/// Returns extension info by its key.
		/// </summary>
		[CanBeNull]
		public virtual TInfo GetExtensionInfo(TKey key)
		{
			return _provider.GetRegisteredElement<TKey, TInfo>(key);
		}

		/// <summary>
		/// Returns registered element infos.
		/// </summary>
		[NotNull]
		public virtual TInfo[] GetExtensionInfos()
		{
			return _provider.GetRegisteredElements<TKey, TInfo>();
		}

		/// <summary>
		/// Returns true, if extension with specified key registered.
		/// </summary>
		public virtual bool IsExtensionRegistered(TKey key)
		{
			return _provider.IsExtensionRegistered<TKey, TInfo>(key);
		}

		/// <summary>
		/// Returns all registered extension instances.
		/// </summary>
		[NotNull]
		public TElement[] GetAllExtensions()
		{
			return
				GetExtensionInfos()
					.Select(info => GetExtension(info.Key))
					.ToArray();
		}

		/// <summary>
		/// Returns extension instance, specified by key.
		/// </summary>
		[CanBeNull]
		public TElement GetExtension(TKey key)
		{
			return _cache.Get(key);
		}
	}
}