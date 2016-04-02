using System;
using System.Reflection;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Extension manager contract.
	/// </summary>
	public interface IExtensionManager
	{
		/// <summary>
		/// Scan assembly.
		/// </summary>
		void Scan(IExtensionAttachmentStrategy strategy, params Assembly[] assemblies);

		/// <summary>
		/// Scan specific type.
		/// </summary>
		void Scan(IExtensionAttachmentStrategy strategy, params Type[] types);
	}
}
