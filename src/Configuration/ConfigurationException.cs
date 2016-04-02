using System;
using System.Runtime.Serialization;

namespace Rsdn.SmartApp.Configuration
{
	/// <summary>
	/// Исключение, возникающее при работе с конфигурацией.
	/// </summary>
	[Serializable]
	public class ConfigurationException : ApplicationException
	{
		/// <summary>
		/// Инициализирует экземпляр переданным сообщением.
		/// </summary>
		public ConfigurationException(string message) : base(message)
		{}

		/// <summary>
		/// Инициализирует экземпляр сообщением по умолчанию.
		/// </summary>
		public ConfigurationException()
			: this(ConfigurationResources.DefaultConfigurationMessage)
		{}

		/// <summary>
		/// Конструктор для десериализации.
		/// </summary>
		protected ConfigurationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{}
	}
}