using System;

namespace CodeJam.Extensibility
{
	/// <summary>
	/// Базовая реализация описания именованного расширения.
	/// </summary>
	public abstract class NamedElementInfo : KeyedElementInfo<string>
	{
		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		protected NamedElementInfo(string name, Type type)
			: base(type, name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));
		}

		/// <summary>
		/// Имя расширения.
		/// </summary>
		public string Name => Key;
	}
}