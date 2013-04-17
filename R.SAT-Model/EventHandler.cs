namespace Rsdn.SmartApp
{
	/// <summary>
	/// Обработчик события без аргументов.
	/// </summary>
	public delegate void EventHandler<TSender>(TSender sender);

	/// <summary>
	/// Обработчик события с аргументами.
	/// </summary>
	public delegate void EventHandler<TSender, TParams>(TSender sender, TParams eventParams);
}