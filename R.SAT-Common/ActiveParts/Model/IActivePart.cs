namespace Rsdn.SmartApp
{
	/// <summary>
	/// Active part interface.
	/// </summary>
	public interface IActivePart
	{
		/// <summary>
		/// Activate part.
		/// </summary>
		void Activate();

		/// <summary>
		/// Passivate part.
		/// </summary>
		void Passivate();
	}
}