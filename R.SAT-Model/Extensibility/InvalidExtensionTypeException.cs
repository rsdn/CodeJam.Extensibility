using System;
using System.Runtime.Serialization;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Исключение, возбуждаемое при неверном типе расширения.
	/// </summary>
	[Serializable]
	public class InvalidExtensionTypeException : ExtensibilityException
	{
		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public InvalidExtensionTypeException() : base("Extensibility error")
		{
		}

		/// <summary>
		/// Инициализирует экземпляр значением сообщения.
		/// </summary>
		public InvalidExtensionTypeException(string message) : base(message)
		{
		}

		/// <summary>
		/// Инициализирует экземпляр значением сообщения и вложенным исключением.
		/// </summary>
		public InvalidExtensionTypeException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Конструктор для сериализации.
		/// Смотри <see cref="Exception(SerializationInfo, StreamingContext)"/>
		/// </summary>
		protected InvalidExtensionTypeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}