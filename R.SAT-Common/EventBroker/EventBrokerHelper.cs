using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reflection;

using JetBrains.Annotations;

namespace Rsdn.SmartApp
{
	/// <summary>
	/// Хелперный класс для работы с Event Broker.
	/// </summary>
	public static class EventBrokerHelper
	{
		/// <summary>
		/// Subscribe to event with delegate for OnNext.
		/// </summary>
		public static IDisposable Subscribe<T>(
			[NotNull] this IEventBroker eventBroker,
			[NotNull] string eventName,
			[NotNull] Action<T> nextAction)
		{
			if (eventBroker == null)
				throw new ArgumentNullException("eventBroker");
			if (eventName == null)
				throw new ArgumentNullException("eventName");
			if (nextAction == null)
				throw new ArgumentNullException("nextAction");

			return eventBroker.Subscribe(eventName, Observer.Create(nextAction));
		}

		/// <summary>
		/// Подписывает на события специально размеченные методы.
		/// </summary>
		public static IDisposable SubscribeEventHandlers(
			[NotNull] object instance,
			[NotNull] IServiceProvider provider)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");
			if (provider == null)
				throw new ArgumentNullException("provider");

			var eventBroker = provider.GetRequiredService<IEventBroker>();
			return 
				instance
					.GetType()
					.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					.SelectMany(
						method => method.GetCustomAttributes<EventHandlerAttribute>(true),
						(method, attr) =>
							WrapException(
								() => eventBroker.SubscribeMethod(attr.EventName, method, instance),
								() => "Error subscribing method '{0}'.".FormatStr(method)))
					.Merge();
		}

		private static T WrapException<T>(Func<T> action, Func<string> message)
		{
			try
			{
				return action();
			}
			catch (Exception ex)
			{
				throw new ApplicationException(message(), ex);
			}
		}

		/// <summary>
		/// Регистрирует специально размеченные источники событий.
		/// </summary>
		public static IDisposable RegisterEventSources(
			[NotNull] object instance,
			[NotNull] IServiceProvider provider)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");
			if (provider == null)
				throw new ArgumentNullException("provider");

			var eventBroker = provider.GetRequiredService<IEventBroker>();

			var instanceType = instance.GetType();
			return
				instanceType
					.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					.SelectMany(
						property => property.GetCustomAttributes<EventSourceAttribute>(true),
						(property, attr) =>
							WrapException(
								() => eventBroker.RegisterProperty(attr.EventName, property, instance),
								() => "Error while registring property '{0}'.".FormatStr(property)))
						.Concat(
							instanceType
								.GetEvents(BindingFlags.Instance | BindingFlags.Public)
								.SelectMany(
									ev => ev.GetCustomAttributes<EventSourceAttribute>(true),
									(ev, attr) =>
										WrapException(
											() => eventBroker.RegisterEvent(attr.EventName, ev, instance),
											() => "Error while registering event '{0}'.".FormatStr(ev))))
						.Merge();
		}

		private static IDisposable SubscribeMethod(
			this IEventBroker eventBroker,
			string eventName,
			MethodInfo methodInfo,
			object instance)
		{
			var parameters = methodInfo.GetParameters();
			if (parameters.Length != 1)
				throw new ApplicationException("Method must have one argument.");

			var argType = parameters[0].ParameterType;
			return
				(IDisposable)Activator
					.CreateInstance(
						typeof (FromBrokerObserver<>).MakeGenericType(argType),
						instance,
						methodInfo,
						eventName,
						eventBroker);
		}

		private static IDisposable RegisterProperty(
			this IEventBroker eventBroker,
			string eventName,
			PropertyInfo property,
			object instance)
		{
			if (!property.PropertyType.IsGenericType
					|| property.PropertyType.GetGenericTypeDefinition() != typeof(IObservable<>))
				throw new ApplicationException("Return type is not IObservable<>.");

			if (!property.CanRead)
				throw new ApplicationException("Missing get accessor.");

			var observable = property.GetValue(instance, null);
			if (observable == null)
				throw new ApplicationException("Return value is null.");

			var argType = property.PropertyType.GetGenericArguments().Single();
			var observer =
				Activator
					.CreateInstance(
						typeof (ToBrokerObserver<>).MakeGenericType(argType),
						eventName,
						eventBroker,
						observable);

			return (IDisposable)observer;
		}

		private static readonly Type _invokerType = typeof(Invoker);
		private static readonly MethodInfo _invokerFireMethod = _invokerType.GetMethod("Fire");

		private static IDisposable RegisterEvent(
			this IEventBroker eventBroker,
			string eventName,
			EventInfo eventInfo,
			object instance)
		{
			var eventHandlerType = eventInfo.EventHandlerType;
			if (eventHandlerType == null)
				throw new ApplicationException();

			var parameters = ReflectionHelper.GetDelegateParams(eventHandlerType);
			if (parameters.Length != 1)
				throw new ApplicationException("Delegate must have one argument.");

			var handler =
				Delegate.CreateDelegate(
					eventHandlerType,
					Activator.CreateInstance(_invokerType, eventName, eventBroker),
					_invokerFireMethod.MakeGenericMethod(parameters[0].ParameterType));

// ReSharper disable AssignNullToNotNullAttribute
			eventInfo.AddEventHandler(instance, handler);

			return Disposable.Create(() => eventInfo.RemoveEventHandler(instance, handler));
// ReSharper restore AssignNullToNotNullAttribute
		}

		#region FromBrokerObserver class
		private class FromBrokerObserver<T> : IObserver<T>, IDisposable
		{
			private readonly object _target;
			private readonly MethodInfo _targetMethod;
			private readonly IDisposable _subscription;

			public FromBrokerObserver(
				object target,
				MethodInfo targetMethod,
				string eventName,
				IEventBroker broker)
			{
				_target = target;
				_targetMethod = targetMethod;
				_subscription = broker.Subscribe(eventName, this);
			}

			public void OnNext(T value)
			{
				_targetMethod.Invoke(_target, new[] {(object)value});
			}

			public void OnError(Exception exception)
			{}

			public void OnCompleted()
			{}

			public void Dispose()
			{
				_subscription.Dispose();
			}
		}
		#endregion

		#region ToBrokerObserver
		private class ToBrokerObserver<T> : IObserver<T>, IDisposable
		{
			private readonly string _eventName;
			private readonly IEventBroker _broker;
			private readonly IDisposable _subscription;

			public ToBrokerObserver(string eventName, IEventBroker broker, IObservable<T> source)
			{
				_eventName = eventName;
				_broker = broker;
				_subscription = source.Subscribe(this);
			}

			public void OnNext(T value)
			{
				_broker.Fire(_eventName, value);
			}

			public void OnError(Exception exception)
			{
			}

			public void OnCompleted()
			{
			}

			public void Dispose()
			{
				_subscription.Dispose();
			}
		}
		#endregion

		#region Invoker class
		private class Invoker
		{
			private readonly string _eventName;
			private readonly IEventBroker _broker;

			public Invoker(string eventName, IEventBroker broker)
			{
				_eventName = eventName;
				_broker = broker;
			}

			[UsedImplicitly]
			public void Fire<T>(T args)
			{
				_broker.Fire(_eventName, args);
			}
		}
		#endregion
	}
}