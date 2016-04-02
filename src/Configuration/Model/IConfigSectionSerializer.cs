using System.Xml;

namespace CodeJam.Extensibility.Configuration
{
	/// <summary>
	/// Сериализатор секции конфигурации.
	/// </summary>
	public interface IConfigSectionSerializer
	{
		/// <summary>
		/// Десериализовать секцию.
		/// </summary>
		object Deserialize(XmlReader reader);

		/// <summary>
		/// Создать секцию по умолчанию.
		/// </summary>
		object CreateDefaultSection();
	}
}