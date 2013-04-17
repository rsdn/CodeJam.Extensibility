using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Интерфейс сервиса, позволяющего публиковать сервисы.
	/// </summary>
	public interface IServicePublisher : IServiceProvider
	{
		/// <summary>
		/// Публикует экземпляр сервиса типа Т.
		/// </summary>
		[NotNull]
		IServiceRegistrationCookie Publish(
			[NotNull] Type serviceType,
			[NotNull] object serviceInstance);

		/// <summary>
		/// Публикует сервис типа Т с отложенной инициализацией.
		/// </summary>
		[NotNull]
		IServiceRegistrationCookie Publish(
			[NotNull] Type serviceType,
			[NotNull] ServiceCreator serviceCreator);

		/// <summary>
		/// Убирает публикацию сервиса.
		/// </summary>
		void Unpublish([NotNull] IServiceRegistrationCookie cookie);
	}
}