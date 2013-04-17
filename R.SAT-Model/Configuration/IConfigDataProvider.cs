using System.Xml;

namespace Rsdn.SmartApp.Configuration
{
	/// <summary>
	/// Поставщик данных с конфигурационной информацией.
	/// </summary>
	public interface IConfigDataProvider
	{
		/// <summary>
		/// Читает данные конфигурации.
		/// </summary>
		/// <remarks>Ридер будет уничтожен вызывающим кодом</remarks>
		XmlReader ReadData();

		/// <summary>
		/// Разрешает include.
		/// </summary>
		IConfigDataProvider ResolveInclude(string path);

		/// <summary>
		/// Происходит при изменении файла конфигурации внешними средствами.
		/// </summary>
		event EventHandler<IConfigDataProvider> ConfigChanged;
	}
}