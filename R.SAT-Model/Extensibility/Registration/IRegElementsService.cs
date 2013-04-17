namespace Rsdn.SmartApp
{
	/// <summary>
	/// Сервис регистрации элементов.
	/// </summary>
	public interface IRegElementsService<TInfo>
	{
		/// <summary>
		/// Зарегистрировать элемент.
		/// </summary>
		void Register(TInfo elementInfo);

		/// <summary>
		/// Получить список зарегистрированных элементов.
		/// </summary>
		TInfo[] GetRegisteredElements();
	}
}