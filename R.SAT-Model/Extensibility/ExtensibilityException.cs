using System;
using System.Runtime.Serialization;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Исключение, возбуждаемое в процессе подключения расширений.
	/// </summary>
	[Serializable]
	public class ExtensibilityException : Exception
	{
		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public ExtensibilityException() : base("Extensibility error")
		{
		}

		/// <summary>
		/// Инициализирует экземпляр значением сообщения.
		/// </summary>
		public ExtensibilityException(string message) : base(message)
		{
		}

		/// <summary>
		/// Инициализирует экземпляр значением сообщения и вложенным исключением.
		/// </summary>
		public ExtensibilityException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Конструктор для сериализации.
		/// Смотри <see cref="Exception(SerializationInfo, StreamingContext)"/>
		/// </summary>
		protected ExtensibilityException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}