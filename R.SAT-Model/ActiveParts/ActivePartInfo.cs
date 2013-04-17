using System;
using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// ќписание active part.
	/// </summary>
	public class ActivePartInfo
	{
		private readonly string _typeName;

		/// <summary>
		/// »нициализирует экземпл€р.
		/// </summary>
		public ActivePartInfo([NotNull] string typeName)
		{
			_typeName = typeName;
			if (typeName == null)
				throw new ArgumentNullException("typeName");
		}

		/// <summary>
		/// “ип, реализующий расширение.
		/// </summary>
		public string TypeName
		{
			get { return _typeName; }
		}
	}
}