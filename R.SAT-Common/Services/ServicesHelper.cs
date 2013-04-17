using System;
using System.ComponentModel.Design;
using System.Reactive.Disposables;
using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Хелперный класс для работы с сервисами.
	/// </summary>
	public static partial class ServicesHelper
	{
		#region IServiceProvider helpers
		/// <summary>
		/// Присваивает экземпляры сервисов специально размеченным полям.
		/// </summary>
		public static void AssignServices(this object instance, IServiceProvider provider)
		{
			AssignHelper.Assign(instance, provider);
		}

		/// <summary>
		/// Возвращает затребованный сервис у системного IServiceProvider.
		/// </summary>
		[CanBeNull]
		public static T GetService<T>(this IServiceProvider provider) where T : class
		{
			return (T)provider.GetService(typeof(T));
		}

		/// <summary>
		/// Возвращает затребованный сервис у системного IServiceProvider.
		/// </summary>
		[NotNull]
		public static T GetRequiredService<T>(this IServiceProvider provider) where T : class
		{
			var svc = (T)provider.GetService(typeof(T));
			if (svc == null)
				throw new ServiceNotFoundException(typeof(T));
			return svc;
		}
		#endregion

		#region IServicePublisher helpers
		/// <summary>
		/// Generic версия <see cref="IServicePublisher.Publish(System.Type,object)"/>
		/// </summary>
		public static IServiceRegistrationCookie Publish<T>(
			[NotNull] this IServicePublisher publisher,
			[NotNull] T serviceInstance)
			where T : class
		{
			if (publisher == null)
				throw new ArgumentNullException("publisher");
			if (serviceInstance == null)
				throw new ArgumentNullException("serviceInstance");

			return publisher.Publish(typeof (T), serviceInstance);
		}

		/// <summary>
		/// Generic версия <see cref="IServicePublisher.Publish(System.Type,object)"/>
		/// </summary>
		public static IDisposable PublishDisposable<T>(
			[NotNull] this IServicePublisher publisher,
			[NotNull] T serviceInstance)
			where T : class
		{
			var cookie = publisher.Publish(serviceInstance);
			return Disposable.Create(() => publisher.Unpublish(cookie));
		}

		/// <summary>
		/// Generic версия <see cref="IServicePublisher.Publish(System.Type,Rsdn.SmartApp.ServiceCreator)"/>
		/// </summary>
		public static IServiceRegistrationCookie Publish<T>(
			[NotNull] this IServicePublisher publisher,
			[NotNull] ServiceCreator<T> serviceCreator)
			where T : class
		{
			if (publisher == null)
				throw new ArgumentNullException("publisher");
			if (serviceCreator == null)
				throw new ArgumentNullException("serviceCreator");

			return publisher.Publish(typeof (T), (type, pub) => serviceCreator(pub));
		}

		/// <summary>
		/// Generic версия <see cref="IServicePublisher.Publish(System.Type,Rsdn.SmartApp.ServiceCreator)"/>
		/// </summary>
		public static IDisposable PublishDisposable<T>(
			[NotNull] this IServicePublisher publisher,
			[NotNull] ServiceCreator<T> serviceCreator)
			where T : class
		{
			var cookie = publisher.Publish(serviceCreator);
			return Disposable.Create(() => publisher.Unpublish(cookie));
		}
		#endregion

		/// <summary>
		/// Типизированная версия <see cref="IServiceContainer.AddService(Type,object)"/>
		/// </summary>
		public static void AddService<T>(this IServiceContainer container, T serviceInstance)
		{
			container.AddService(typeof(T), serviceInstance);
		}

		/// <summary>
		/// Creates service publishing strategy.
		/// </summary>
		public static IExtensionAttachmentStrategy CreateServiceStrategy(
			[NotNull] IServicePublisher publisher)
		{
			if (publisher == null)
				throw new ArgumentNullException("publisher");
			return new ServicePublishingStrategy(publisher);
		}
	}

	/// <summary>
	/// Generic версия <see cref="ServiceCreator"/>
	/// </summary>
	public delegate T ServiceCreator<T>(IServicePublisher publisher);
}