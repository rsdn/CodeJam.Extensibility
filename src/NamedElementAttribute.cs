using System;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Базовая реализация атрибута, которым можно помечать именованные расширения.
	/// </summary>
	public abstract class NamedElementAttribute : Attribute
	{
		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		protected NamedElementAttribute(string name)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			Name = name;
		}

		/// <summary>
		/// Имя расширения.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Описание.
		/// </summary>
		public string Description { get; set; }
	}
}