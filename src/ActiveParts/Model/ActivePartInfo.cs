using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// ќписание active part.
	/// </summary>
	public class ActivePartInfo
	{
		/// <summary>
		/// »нициализирует экземпл€р.
		/// </summary>
		public ActivePartInfo([NotNull] string typeName)
		{
			TypeName = typeName;
			if (typeName == null)
				throw new ArgumentNullException(nameof(typeName));
		}

		/// <summary>
		/// “ип, реализующий расширение.
		/// </summary>
		public string TypeName { get; }
	}
}