using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Базовая реализация <see cref="IServicePublisher"/>
	/// </summary>
	public class ServiceManager : IServicePublisher, IDisposable
	{
		private readonly Dictionary<ServiceCookie, Type> _byCookieMap =
			new Dictionary<ServiceCookie, Type>();

		private readonly Dictionary<Type, IServiceInstanceProvider> _byTypeMap =
			new Dictionary<Type, IServiceInstanceProvider>();

		private readonly object _changeLockFlag = new object();
		private readonly IServiceProvider[] _parentProviders;

		/// <summary>
		/// Инициализирует экземпляр без публикации <see cref="IServicePublisher"/>.
		/// </summary>
		/// <param name="parentProviders">поставщик - родитель</param>
		public ServiceManager(params IServiceProvider[] parentProviders)
			: this(false, parentProviders)
		{}

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		/// <param name="parentProviders">поставщик - родитель</param>
		/// <param name="publishPublisher">признак необходимости публикации
		/// <see cref="IServicePublisher"/></param>
		public ServiceManager(bool publishPublisher, params IServiceProvider[] parentProviders)
		{
			_parentProviders = parentProviders;
			var i = 0;
			foreach (var provider in parentProviders)
			{
				if (provider == null)
					throw new ArgumentNullException("parentProviders[" + i + "]");
				i++;
			}
			if (publishPublisher)
				Publish(typeof (IServicePublisher), this);
		}

		#region IServicePublisher Members
		/// <summary>
		/// Публикует экземпляр сервиса типа Т.
		/// </summary>
		public IServiceRegistrationCookie Publish(Type serviceType, object serviceInstance)
		{
			if (serviceType == null)
				throw new ArgumentNullException("serviceType");
			if (serviceInstance == null)
				throw new ArgumentNullException("serviceInstance");
			return Publish(serviceType, new SvcInstanceProvider(serviceInstance));
		}

		/// <summary>
		/// Публикует сервис типа Т с отложенной инициализацией.
		/// </summary>
		public IServiceRegistrationCookie Publish(Type serviceType, ServiceCreator serviceCreator)
		{
			if (serviceType == null)
				throw new ArgumentNullException("serviceType");
			if (serviceCreator == null)
				throw new ArgumentNullException("serviceCreator");

			return Publish(serviceType, new SvcCreatorProvider(serviceType, this, serviceCreator));
		}

		/// <summary>
		/// Убирает публикацию сервиса.
		/// </summary>
		public void Unpublish(IServiceRegistrationCookie cookie)
		{
			if (cookie == null)
				throw new ArgumentNullException("cookie");

			var svcCookie = (ServiceCookie)cookie;
			if (!_byCookieMap.ContainsKey(svcCookie))
				throw new ArgumentException(ServiceResources.ServiceManagerCookieIinvalid, "cookie");
			lock (_changeLockFlag)
			{
				_byTypeMap.Remove(_byCookieMap[svcCookie]);
				_byCookieMap.Remove(svcCookie);
			}
		}

		/// <summary>
		/// Возвращает сервис, реализующий интерфейс T
		/// </summary>
		[CanBeNull]
		public virtual object GetService([NotNull] Type serviceType)
		{
			if (serviceType == null)
				throw new ArgumentNullException("serviceType");

			IServiceInstanceProvider instanceProvider;
			return
				!_byTypeMap.TryGetValue(serviceType, out instanceProvider)
					? GetParentService(serviceType)
					: instanceProvider.GetInstance();
		}
		#endregion

		private IServiceRegistrationCookie Publish(Type svcType, IServiceInstanceProvider instanceProvider)
		{
			if (_byTypeMap.ContainsKey(svcType))
				throw new ArgumentException("Service " + svcType.FullName
					+ " already published");
			lock (_changeLockFlag)
			{
				var cookie = new ServiceCookie();
				_byTypeMap.Add(svcType, instanceProvider);
				_byCookieMap.Add(cookie, svcType);
				return cookie;
			}
		}

		/// <summary>
		/// Перечисляет уже созданные экземпляры сервисов.
		/// </summary>
		public IEnumerable<object> GetCreatedInstances()
		{
			return
				_byTypeMap
					.Values
					.Where(handler => handler.IsInstanceCreated)
					.Select(handler => handler.GetInstance());
		}

		private object GetParentService(Type serviceType)
		{
			return
				_parentProviders != null
					? _parentProviders
						.Select(sp => sp.GetService(serviceType))
						.FirstOrDefault(svc => svc != null)
					: null;
		}

		#region ServiceCookie class
		private class ServiceCookie : IServiceRegistrationCookie
		{
			private readonly Guid _guid = Guid.NewGuid();

			public override int GetHashCode()
			{
				return _guid.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				if (this == obj)
					return true;
				var serviceCookie = obj as ServiceCookie;
				if (serviceCookie == null)
					return false;
				return _guid == serviceCookie._guid;
			}
		}
		#endregion

		#region Service handlers

		#region Nested type: IServiceInstanceProvider
		private interface IServiceInstanceProvider
		{
			bool IsInstanceCreated { get; }
			object GetInstance();
		}
		#endregion

		#region Nested type: SvcCreatorProvider
		private class SvcCreatorProvider : IServiceInstanceProvider
		{
			private readonly Type _serviceType;
			private readonly IServicePublisher _publisher;
			private readonly ServiceCreator _serviceCreator;
			private volatile object _svcInstance;

			public SvcCreatorProvider(Type serviceType, IServicePublisher publisher, ServiceCreator serviceCreator)
			{
				_serviceType = serviceType;
				_publisher = publisher;
				_serviceCreator = serviceCreator;
			}

			#region IServiceInstanceProvider Members
			public bool IsInstanceCreated
			{
				get { return _svcInstance != null; }
			}

			public object GetInstance()
			{
				if (_svcInstance == null)
					lock (this)
						if (_svcInstance == null)
						{
							_svcInstance = _serviceCreator(_serviceType, _publisher);
							if (_svcInstance == null)
								throw new ArgumentException("Could not create instance of a service");
						}
				return _svcInstance;
			}
			#endregion
		}
		#endregion

		#region Nested type: SvcInstanceProvider
		private class SvcInstanceProvider : IServiceInstanceProvider
		{
			private readonly object _svcInstance;

			public SvcInstanceProvider(object svcInstance)
			{
				_svcInstance = svcInstance;
			}

			#region IServiceInstanceProvider Members
			public bool IsInstanceCreated
			{
				get { return true; }
			}

			public object GetInstance()
			{
				return _svcInstance;
			}
			#endregion
		}
		#endregion

		#endregion

		#region Implementation of IDisposable
		private bool _disposingNow;

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (_disposingNow)
				return;
			_disposingNow = true;
			using (Disposable.Create(() => _disposingNow = false))
				_byTypeMap
					.Values
					.Where(prov => prov.IsInstanceCreated)
					.Select(prov => prov.GetInstance())
					.Where(inst => inst != this) // Do not dispose self
					.OfType<IDisposable>()
					.DisposeAll();
		}
		#endregion
	}
}