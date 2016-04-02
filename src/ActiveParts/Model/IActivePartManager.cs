using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Менеджер active parts.
	/// </summary>
	public interface IActivePartManager
	{
		/// <summary>
		/// Current state.
		/// </summary>
		ActivePartManagerState State { get; }

		/// <summary>
		/// Получить экземпляр active part.
		/// </summary>
		[NotNull]
		object GetPartInstance([NotNull] Type type);

		/// <summary>
		/// Activate all registered parts.
		/// </summary>
		void Activate();

		/// <summary>
		/// Passivate all active parts.
		/// </summary>
		void Passivate();
	}
}