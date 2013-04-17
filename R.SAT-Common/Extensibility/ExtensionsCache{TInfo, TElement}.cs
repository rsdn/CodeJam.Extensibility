using System;
using System.Linq;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Extension instances cache.
	/// Thread safe.
	/// </summary>
	public class ExtensionsCache<TInfo, TElement>
		where TInfo : ElementInfo
	{
		private readonly IServiceProvider _provider;
		private readonly Func<TInfo, TElement> _instanceCreator;
		private readonly InstancingCustomParam[] _customParams;

		private readonly Lazy<TElement[]> _cache;

		/// <summary>
		/// Initialize instance with specified instance creator.
		/// </summary>
		public ExtensionsCache(
			[NotNull] IServiceProvider provider,
			[CanBeNull] Func<TInfo, TElement> instanceCreator)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");
			_provider = provider;
			_customParams = EmptyArray<InstancingCustomParam>.Value;
			_instanceCreator = instanceCreator;
			_cache =
				new Lazy<TElement[]>(
					_instanceCreator != null
						? (Func<TElement[]>)CreateElementsByCreator
						: CreateElementsByHelper);
		}

		/// <summary>
		/// Initialize instance.
		/// </summary>
		public ExtensionsCache(
				[NotNull] IServiceProvider provider,
				params InstancingCustomParam[] customParams)
			: this(provider, (Func<TInfo, TElement>)null)
		{
			_customParams = customParams;
		}

		private TElement[] CreateElementsByHelper()
		{
			return
				GetExtensionInfos()
					.Select(info => info.Type.CreateInstance<TElement>(_provider, _customParams))
					.ToArray();
		}

		private TElement[] CreateElementsByCreator()
		{
			return
				GetExtensionInfos()
					.Select(info => _instanceCreator(info))
					.ToArray();
		}

		/// <summary>
		/// Returns registered element infos.
		/// </summary>
		public virtual TInfo[] GetExtensionInfos()
		{
			return _provider.GetRegisteredElements<TInfo>();
		}

		/// <summary>
		/// Returns all extension instances.
		/// </summary>
		public TElement[] GetAllExtensions()
		{
			return _cache.Value;
		}
	}
}