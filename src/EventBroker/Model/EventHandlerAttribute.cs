using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility.EventBroker
{
	/// <summary>
	/// Атрибут, помечающий обработчик события.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	[MeansImplicitUse]
	public class EventHandlerAttribute : Attribute
	{
		/// <summary>
		/// Инициализиурет экземпляр.
		/// </summary>
		public EventHandlerAttribute(string eventName)
		{
			EventName = eventName;
		}

		/// <summary>
		/// Имя события.
		/// </summary>
		public string EventName { get; }
	}
}