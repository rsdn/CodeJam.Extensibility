using System.Reflection;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Стратегия подключения расширения.
	/// </summary>
	public interface IExtensionAttachmentStrategy
	{
		/// <summary>
		/// Attach extension.
		/// </summary>
		void Attach(ExtensionAttachmentContext context, CustomAttributeData attribute);
	}
}
