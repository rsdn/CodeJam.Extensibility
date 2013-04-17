using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Helper methods for extensibility infa.
	/// </summary>
	public static class ExtensibilityHelper
	{
		#region CreateStrategy()
		/// <summary>
		/// Creates strategy with supplied publisher and element info creator.
		/// </summary>
		[NotNull]
		public static IExtensionAttachmentStrategy CreateStrategy<TInfo, TAttr>(
			[NotNull] IServicePublisher publisher,
			[NotNull] Func<ExtensionAttachmentContext, TAttr, TInfo> infoCreator)
			where TAttr : Attribute
			where TInfo : class
		{
			if (publisher == null)
				throw new ArgumentNullException("publisher");
			if (infoCreator == null)
				throw new ArgumentNullException("infoCreator");

			return new DelegateStrategy<TInfo, TAttr>(publisher, infoCreator, null);
		}

		/// <summary>
		/// Creates strategy with supplied publisher and keyed element info creator.
		/// </summary>
		[NotNull]
		public static IExtensionAttachmentStrategy CreateStrategy<TKey, TInfo, TAttr>(
				[NotNull] IServicePublisher publisher,
				[NotNull] Func<ExtensionAttachmentContext, TAttr, TInfo> infoCreator)
			where TAttr : Attribute
			where TInfo : class, IKeyedElementInfo<TKey>
			where TKey : class
		{
			if (publisher == null)
				throw new ArgumentNullException("publisher");
			if (infoCreator == null)
				throw new ArgumentNullException("infoCreator");

			return new DelegateStrategy<TKey, TInfo, TAttr>(publisher, infoCreator, null);
		}

		/// <summary>
		/// Creates strategy with supplied publisher, element info creator and with
		/// contract implementation check.
		/// </summary>
		[NotNull]
		public static IExtensionAttachmentStrategy CreateStrategy<TInfo, TAttr, TElement>(
			[NotNull] IServicePublisher publisher,
			[NotNull] Func<ExtensionAttachmentContext, TAttr, TInfo> infoCreator)
			where TAttr : Attribute
			where TInfo : class
		{
			if (publisher == null)
				throw new ArgumentNullException("publisher");
			if (infoCreator == null)
				throw new ArgumentNullException("infoCreator");

			return CreateStrategy(publisher, infoCreator, typeof (TElement));
		}

		/// <summary>
		/// Creates strategy with supplied publisher, keyed element info creator and with
		/// contract implementation check.
		/// </summary>
		[NotNull]
		public static IExtensionAttachmentStrategy CreateStrategy<TKey, TInfo, TAttr, TElement>(
			[NotNull] IServicePublisher publisher,
			[NotNull] Func<ExtensionAttachmentContext, TAttr, TInfo> infoCreator)
			where TAttr : Attribute
			where TInfo : class, IKeyedElementInfo<TKey>
			where TKey : class
		{
			if (publisher == null)
				throw new ArgumentNullException("publisher");
			if (infoCreator == null)
				throw new ArgumentNullException("infoCreator");

			return CreateStrategy<TKey, TInfo, TAttr>(publisher, infoCreator, typeof(TElement));
		}

		/// <summary>
		/// Creates strategy with supplied publisher, element info creator and with
		/// contract implementation check.
		/// </summary>
		[NotNull]
		public static IExtensionAttachmentStrategy CreateStrategy<TInfo, TAttr>(
				[NotNull] IServicePublisher publisher,
				[NotNull] Func<ExtensionAttachmentContext, TAttr, TInfo> infoCreator,
				[NotNull] Type elementType)
			where TAttr : Attribute
			where TInfo : class
		{
			if (publisher == null)
				throw new ArgumentNullException("publisher");
			if (infoCreator == null)
				throw new ArgumentNullException("infoCreator");
			if (elementType == null)
				throw new ArgumentNullException("elementType");

			return new DelegateStrategy<TInfo, TAttr>(publisher, infoCreator, elementType);
		}

		/// <summary>
		/// Creates strategy with supplied publisher, element info creator and with
		/// contract implementation check.
		/// </summary>
		[NotNull]
		public static IExtensionAttachmentStrategy CreateStrategy<TKey, TInfo, TAttr>(
				[NotNull] IServicePublisher publisher,
				[NotNull] Func<ExtensionAttachmentContext, TAttr, TInfo> infoCreator,
				[NotNull] Type elementType)
			where TAttr : Attribute
			where TInfo : class, IKeyedElementInfo<TKey> where TKey : class
		{
			if (publisher == null)
				throw new ArgumentNullException("publisher");
			if (infoCreator == null)
				throw new ArgumentNullException("infoCreator");
			if (elementType == null)
				throw new ArgumentNullException("elementType");

			return new DelegateStrategy<TKey, TInfo, TAttr>(publisher, infoCreator, elementType);
		}

		/// <summary>
		/// Creates strategy with supplied service provider and element info creator.
		/// </summary>
		[NotNull]
		public static IExtensionAttachmentStrategy CreateStrategy<TInfo, TAttr>(
			[NotNull] this IServiceProvider provider,
			[NotNull] Func<ExtensionAttachmentContext, TAttr, TInfo> infoCreator)
			where TAttr : Attribute
			where TInfo : class
		{
			if (provider == null)
				throw new ArgumentNullException("provider");
			var publisher = provider.GetRequiredService<IServicePublisher>();
			return CreateStrategy(publisher, infoCreator);
		}

		/// <summary>
		/// Creates strategy with supplied service provider and keyed element info creator.
		/// </summary>
		[NotNull]
		public static IExtensionAttachmentStrategy CreateStrategy<TKey, TInfo, TAttr>(
			[NotNull] this IServiceProvider provider,
			[NotNull] Func<ExtensionAttachmentContext, TAttr, TInfo> infoCreator)
			where TAttr : Attribute
			where TInfo : class, IKeyedElementInfo<TKey>
			where TKey : class
		{
			if (provider == null)
				throw new ArgumentNullException("provider");
			var publisher = provider.GetRequiredService<IServicePublisher>();
			return CreateStrategy<TKey, TInfo, TAttr>(publisher, infoCreator);
		}

		/// <summary>
		/// Creates strategy with supplied service provider and element info creator.
		/// </summary>
		[NotNull]
		public static IExtensionAttachmentStrategy CreateStrategy<TInfo, TAttr>(
			[NotNull] this IServiceProvider provider,
			[NotNull] Func<Type, TInfo> infoCreator)
			where TAttr : Attribute
			where TInfo : class
		{
			if (infoCreator == null)
				throw new ArgumentNullException("infoCreator");
			return CreateStrategy<TInfo, TAttr>(provider, (ctx, attr) => infoCreator(ctx.Type));
		}

		/// <summary>
		/// Creates strategy with supplied service provider and keyed element info creator.
		/// </summary>
		[NotNull]
		public static IExtensionAttachmentStrategy CreateStrategy<TKey, TInfo, TAttr>(
			[NotNull] this IServiceProvider provider,
			[NotNull] Func<Type, TInfo> infoCreator)
			where TAttr : Attribute
			where TInfo : class, IKeyedElementInfo<TKey>
			where TKey : class
		{
			if (infoCreator == null)
				throw new ArgumentNullException("infoCreator");
			return CreateStrategy<TKey, TInfo, TAttr>(provider, (ctx, attr) => infoCreator(ctx.Type));
		}

		#region DelegateStrategy classes
		private class DelegateStrategy<TInfo, TAttr> : RegElementsStrategy<TInfo, TAttr>
			where TAttr : Attribute
			where TInfo : class
		{
			private readonly Func<ExtensionAttachmentContext, TAttr, TInfo> _creator;
			private readonly Type _elementType;

			public DelegateStrategy(
					[NotNull] IServicePublisher publisher,
					[NotNull] Func<ExtensionAttachmentContext, TAttr, TInfo> creator,
					[CanBeNull] Type elementType)
				: base(publisher)
			{
				if (publisher == null)
					throw new ArgumentNullException("publisher");
				_creator = creator;
				_elementType = elementType;
			}

			public override TInfo CreateElement(
				ExtensionAttachmentContext context,
				TAttr attr)
			{
				if (_elementType != null && !_elementType.IsAssignableFrom(context.Type))
					throw new ExtensibilityException(
						"Type '{0}' must inherit/implement '{1}'"
							.FormatStr(context.Type, _elementType));
				return _creator(context, attr);
			}
		}

		private class DelegateStrategy<TKey, TInfo, TAttr> : RegKeyedElementsStrategy<TKey, TInfo, TAttr>
			where TAttr : Attribute
			where TInfo : class, IKeyedElementInfo<TKey>
			where TKey : class
		{
			private readonly Func<ExtensionAttachmentContext, TAttr, TInfo> _creator;
			private readonly Type _elementType;

			public DelegateStrategy(
					[NotNull] IServicePublisher publisher,
					[NotNull] Func<ExtensionAttachmentContext, TAttr, TInfo> creator,
					[CanBeNull] Type elementType)
				: base(publisher)
			{
				if (publisher == null)
					throw new ArgumentNullException("publisher");
				_creator = creator;
				_elementType = elementType;
			}

			public override TInfo CreateElement(ExtensionAttachmentContext context, TAttr attr)
			{
				if (_elementType != null && !_elementType.IsAssignableFrom(context.Type))
					throw new ExtensibilityException(
						"Type '{0}' must inherit/implement '{1}'"
							.FormatStr(context.Type, _elementType));
				return _creator(context, attr);
			}
		}
		#endregion

		#endregion

		#region ScanExtensions()
		private static IDisposable ReflectionResolverScope()
		{
			ResolveEventHandler resolver = (s, a) => Assembly.ReflectionOnlyLoad(a.Name);
			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += resolver;
			return
				Disposable.Create(
					() => AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= resolver);
		}

		/// <summary>
		/// Load assemblies for reflection only by its names and scans.
		/// </summary>
		public static void ReflectionOnlyScan(
			[NotNull] this IExtensionManager manager,
			IExtensionAttachmentStrategy strategy,
			params string[] asmNames)
		{
			if (manager == null)
				throw new ArgumentNullException("manager");

			using (ReflectionResolverScope())
				manager.Scan(
					strategy,
					asmNames.Select(name => Assembly.ReflectionOnlyLoad(name)).ToArray());
		}

		/// <summary>
		/// Load assemblies for reflection only by its file names and scans.
		/// </summary>
		public static void ReflectionOnlyFromScan(
			[NotNull] this IExtensionManager manager,
			IExtensionAttachmentStrategy strategy,
			params string[] asmFiles)
		{
			if (manager == null)
				throw new ArgumentNullException("manager");

			using (ReflectionResolverScope())
				manager.Scan(
					strategy,
					asmFiles.Select(name => Assembly.ReflectionOnlyLoadFrom(name)).ToArray());
		}

		/// <summary>
		/// Scan extensions in supplied types.
		/// </summary>
		public static IExtensionManager ScanExtensions(
			[NotNull] this IServiceProvider provider,
			[NotNull] Type[] types,
			params IExtensionAttachmentStrategy[] strategies)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");
			if (types == null)
				throw new ArgumentNullException("types");
			var extMgr = new ExtensionManager(provider);
			foreach (var strategy in strategies)
				extMgr.Scan(strategy, types);
			return extMgr;
		}

		/// <summary>
		/// Scan extensions in supplied assembly.
		/// </summary>
		public static IExtensionManager ScanExtensions(
			[NotNull] this IServiceProvider provider,
			[NotNull] Assembly assembly,
			params IExtensionAttachmentStrategy[] strategies)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");
			return ScanExtensions(provider, assembly.GetTypes(), strategies);
		}

		/// <summary>
		/// Scan extensions in calling assembly.
		/// </summary>
		public static IExtensionManager ScanExtensions(
			[NotNull] this IServiceProvider provider,
			params IExtensionAttachmentStrategy[] strategies)
		{
			return ScanExtensions(provider, Assembly.GetCallingAssembly(), strategies);
		}

		/// <summary>
		/// Scan extensions in supplied types.
		/// </summary>
		public static IExtensionManager ReflectionOnlyScanExtensions(
			[NotNull] this IServiceProvider provider,
			[NotNull] string[] asmNames,
			params IExtensionAttachmentStrategy[] strategies)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");
			var extMgr = new ExtensionManager(provider);
			foreach (var strategy in strategies)
				extMgr.ReflectionOnlyScan(strategy, asmNames);
			return extMgr;
		}
		#endregion

		#region Registration services helpers
		///<summary>
		/// Returns registration service.
		///</summary>
		[CanBeNull]
		public static IRegElementsService<TInfo> GetRegService<TInfo>([NotNull] this IServiceProvider provider)
		{
			return provider.GetService<IRegElementsService<TInfo>>();
		}

		///<summary>
		/// Returns registration service.
		///</summary>
		[CanBeNull]
		public static IRegKeyedElementsService<TKey, TInfo> GetRegService<TKey, TInfo>(
			[NotNull] this IServiceProvider provider)
			where TInfo : IKeyedElementInfo<TKey>
		{
			return provider.GetService<IRegKeyedElementsService<TKey, TInfo>>();
		}

		/// <summary>
		/// Return registered elements of supplied element info type.
		/// </summary>
		public static TInfo[] GetRegisteredElements<TInfo>([NotNull] this IServiceProvider provider)
		{
			var svc = provider.GetRegService<TInfo>();
			return
				svc == null
					? EmptyArray<TInfo>.Value
					: svc.GetRegisteredElements();
		}

		/// <summary>
		/// Return registered elements of supplied element info type.
		/// </summary>
		public static TInfo[] GetRegisteredElements<TKey, TInfo>([NotNull] this IServiceProvider provider)
			where TInfo : IKeyedElementInfo<TKey>
		{
			var svc = provider.GetRegService<TKey, TInfo>();
			return
				svc == null
					? EmptyArray<TInfo>.Value
					: svc.GetRegisteredElements();
		}

		/// <summary>
		/// Returns registered element by its key.
		/// </summary>
		public static TInfo GetRegisteredElement<TKey, TInfo>(
			[NotNull] this IServiceProvider provider,
			TKey key)
			where TInfo : class, IKeyedElementInfo<TKey>
		{
			if (provider == null)
				throw new ArgumentNullException("provider");
			var svc = provider.GetRegService<TKey, TInfo>();
			return
				svc == null
					? null
					: svc.GetElement(key);
		}

		/// <summary>
		/// Returns true, if keyed extension with specified key registered.
		/// </summary>
		public static bool IsExtensionRegistered<TKey, TInfo>(
				[NotNull] this IServiceProvider provider,
				TKey key)
			where TInfo : IKeyedElementInfo<TKey>
		{
			if (provider == null)
				throw new ArgumentNullException("provider");
			var svc = provider.GetRegService<TKey, TInfo>();
			return svc != null && svc.ContainsElement(key);
		}
		#endregion
	}
}