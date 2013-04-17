namespace Rsdn.SmartApp
{
	/// <summary>
	/// <see cref="IActivePartManager"/> state.
	/// </summary>
	public enum ActivePartManagerState
	{
		/// <summary>
		/// Manager passivated.
		/// </summary>
		Passivated,

		/// <summary>
		/// Manager in process of activation.
		/// </summary>
		Activating,

		/// <summary>
		/// Manager activated.
		/// </summary>
		Activated,

		/// <summary>
		/// Manager in process of passivation.
		/// </summary>
		Passivating,

		/// <summary>
		/// Manager in invalid state.
		/// </summary>
		Invalid
	}
}