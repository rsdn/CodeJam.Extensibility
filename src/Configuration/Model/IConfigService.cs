namespace Rsdn.SmartApp.Configuration
{
	/// <summary>
	/// Сервис конфигурации.
	/// </summary>
	public interface IConfigService
	{
		/// <summary>
		/// Получить содержимое секции.
		/// </summary>
		T GetSection<T>();

		/// <summary>
		/// Вызывается при изменении конфигурации.
		/// </summary>
		event EventHandler<IConfigService> ConfigChanged;
	}
}