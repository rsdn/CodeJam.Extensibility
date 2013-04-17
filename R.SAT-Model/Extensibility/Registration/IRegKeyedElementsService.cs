namespace Rsdn.SmartApp
{
	/// <summary>
	/// Сервис регистрации именованных элементов.
	/// </summary>
	public interface IRegKeyedElementsService<TKey, TInfo> : IRegElementsService<TInfo>
		where TInfo : IKeyedElementInfo<TKey>
	{
		/// <summary>
		/// Проверяет, есть ли зарегистрированный элемент с указанным именем.
		/// </summary>
		bool ContainsElement(TKey key);

		/// <summary>
		/// Возвращает элемент по его ключу.
		/// </summary>
		TInfo GetElement(TKey key);
	}
}