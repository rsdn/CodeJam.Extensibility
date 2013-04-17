using System.Reflection;

namespace Rsdn.SmartApp
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
