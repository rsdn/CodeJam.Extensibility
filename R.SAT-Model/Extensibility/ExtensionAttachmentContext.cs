using System;
using System.Reflection;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Extension attachment context.
	/// </summary>
	public class ExtensionAttachmentContext : IServiceProvider
	{
		private readonly IServiceProvider _provider;
		private readonly IExtensionManager _extensionManager;
		private readonly Assembly _assembly;
		private readonly Type _type;

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
			_extensionManager = extensionManager;
			_assembly = assembly;
			_type = type;
		}

		/// <summary>
		/// Current extension manager.
		/// </summary>
		public IExtensionManager ExtensionManager
		{
			get { return _extensionManager; }
		}

		/// <summary>
		/// Assembly for assembly level attribute.
		/// </summary>
		public Assembly Assembly
		{
			get { return _assembly; }
		}

		/// <summary>
		/// Type.
		/// </summary>
		public Type Type
		{
			get { return _type; }
		}

		/// <summary>
		/// Returns true, if scan on assembly level.
		/// </summary>
		public bool IsAssemblyLevel
		{
			get { return Type == null; }
		}

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