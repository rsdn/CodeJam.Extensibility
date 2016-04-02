using System;
using System.Reflection;

using Rsdn.SmartApp;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Extension attachment context.
	/// </summary>
	public class ExtensionAttachmentContext : IServiceProvider
	{
		private readonly IServiceProvider _provider;

		/// <summary>
		/// Initialize instance by assembly.
		/// </summary>
		public ExtensionAttachmentContext(
				IServiceProvider provider,
				IExtensionManager extensionManager,
				Assembly assembly)
			: this(provider, extensionManager, assembly, null)
		{}

		/// <summary>
		/// Initialize instance by type.
		/// </summary>
		public ExtensionAttachmentContext(
			IServiceProvider provider,
			IExtensionManager extensionManager,
			Assembly assembly,
			Type type)
		{
			_provider = provider;
			ExtensionManager = extensionManager;
			Assembly = assembly;
			Type = type;
		}

		/// <summary>
		/// Current extension manager.
		/// </summary>
		public IExtensionManager ExtensionManager { get; }

		/// <summary>
		/// Assembly for assembly level attribute.
		/// </summary>
		public Assembly Assembly { get; }

		/// <summary>
		/// Type.
		/// </summary>
		public Type Type { get; }

		/// <summary>
		/// Returns true, if scan on assembly level.
		/// </summary>
		public bool IsAssemblyLevel => Type == null;

		/// <summary>
		/// Gets the service object of the specified type.
		/// </summary>
		/// <returns>
		/// A service object of type <paramref name="serviceType"/>.-or- null if there is no service object of type <paramref name="serviceType"/>.
		/// </returns>
		public object GetService(Type serviceType)
		{
			return _provider.GetService(serviceType);
		}
	}
}