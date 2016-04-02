namespace CodeJam.Extensibility.Registration
{
	/// <summary>
	/// Интерфейс именованного элемента.
	/// </summary>
	public interface IKeyedElementInfo<out TKey>
	{
		/// <summary>
		/// Ключ.
		/// </summary>
		TKey Key { get; }
	}
}