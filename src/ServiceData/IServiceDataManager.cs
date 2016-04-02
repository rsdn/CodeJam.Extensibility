using System;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Менеджер персистентных служебных данных.
	/// </summary>
	public interface IServiceDataManager
	{
		/// <summary>
		/// Вызывается при изменении свойства в данных.
		/// </summary>
		event PropertyChangedHandler PropertyChanged;

		/// <summary>
		/// Получить экземпляр сервисных данных.
		/// </summary>
		T GetServiceDataInstance<T>() where T : class;

		/// <summary>
		/// Сбросить кеш экземпляров.
		/// </summary>
		void ResetCache();
	}

	/// <summary>
	/// Обработчки события изменеия свойства.
	/// </summary>
	public delegate void PropertyChangedHandler(
		IServiceDataManager manager,
		Type ifaceType,
		object dataInstance,
		string propertyName);
}
