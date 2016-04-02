namespace CodeJam.Extensibility
{
	/// <summary>
	/// Обработчик события без аргументов.
	/// </summary>
	public delegate void EventHandler<in TSender>(TSender sender);

	/// <summary>
	/// Обработчик события с аргументами.
	/// </summary>
	public delegate void EventHandler<in TSender, in TParams>(TSender sender, TParams eventParams);
}