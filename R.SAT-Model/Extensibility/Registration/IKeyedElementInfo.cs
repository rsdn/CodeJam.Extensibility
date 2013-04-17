namespace Rsdn.SmartApp
{
	/// <summary>
	/// Интерфейс именованного элемента.
	/// </summary>
	public interface IKeyedElementInfo<TKey>
	{
		/// <summary>
		/// Ключ.
		/// </summary>
		TKey Key { get; }
	}
}