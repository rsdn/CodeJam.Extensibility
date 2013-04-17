using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Атрибут, помечающий обработчик события.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	[MeansImplicitUse]
	public class EventHandlerAttribute : Attribute
	{
		private readonly string _eventName;

		/// <summary>
		/// Инициализиурет экземпляр.
		/// </summary>
		public EventHandlerAttribute(string eventName)
		{
			_eventName = eventName;
		}

		/// <summary>
		/// Имя события.
		/// </summary>
		public string EventName
		{
			get { return _eventName; }
		}
	}
}