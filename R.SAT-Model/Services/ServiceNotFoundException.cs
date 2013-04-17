using System;
using System.Runtime.Serialization;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Выбрасывается при отсутствии требуемого сервиса.
	/// </summary>
	[Serializable]
	public class ServiceNotFoundException : Exception
	{
		/// <summary>
		/// Для сериализации.
		/// </summary>
		protected ServiceNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public ServiceNotFoundException() : base("Required service not found.")
		{
		}

		/// <summary>
		/// Инициализирует экземпляр с указанием сообщения.
		/// </summary>
		public ServiceNotFoundException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Инициализирует экземпляр с указанием типа отсутствующего сервиса.
		/// </summary>
		public ServiceNotFoundException(Type serviceContract)
			: base("Required service '" + serviceContract.FullName + "' not found.")
		{
		}
	}
}