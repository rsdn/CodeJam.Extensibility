using System;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Брокер событий.
	/// </summary>
	public interface IEventBroker
	{
		/// <summary>
		/// Оповестить подписчиков о событии.
		/// </summary>
		void Fire<T>(string eventName, T arg);

		/// <summary>
		/// Подписаться на событие.
		/// </summary>
		IDisposable Subscribe<T>(string eventName, IObserver<T> observer);
	}
}