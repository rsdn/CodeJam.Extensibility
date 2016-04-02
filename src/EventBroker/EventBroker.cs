using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;

using CodeJam.Extensibility.Threading;

using JetBrains.Annotations;

namespace CodeJam.Extensibility.EventBroker
{
	/// <summary>
	/// Стандартная реализация <see cref="IEventBroker"/>.
	/// </summary>
	[Localizable(false)]
	public class EventBroker : IEventBroker
	{
		private readonly Dictionary<string, EventLinker> _events = new Dictionary<string, EventLinker>();
		private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

		#region Implementation of IEventBroker
		/// <summary>
		/// Fire event.
		/// </summary>
		public void Fire<T>([NotNull] string eventName, T arg)
		{
			if (eventName == null)
				throw new ArgumentNullException(nameof(eventName));

			IObserver<T>[] observers;
			using (_rwLock.GetReaderLock())
			{
				var eventLinker = GetEventLinker<T>(eventName);
				if (eventLinker == null)
					return;
				observers = eventLinker.Observers.Cast<IObserver<T>>().ToArray();
			}

			foreach (var observer in observers)
				observer.OnNext(arg);
		}

		/// <summary>
		/// Subscribe to broker events.
		/// </summary>
		public IDisposable Subscribe<T>([NotNull] string eventName, [NotNull] IObserver<T> observer)
		{
			if (eventName == null)
				throw new ArgumentNullException(nameof(eventName));
			if (observer == null)
				throw new ArgumentNullException(nameof(observer));

			using (_rwLock.GetWriterLock())
			{
				var eventLinker = EnsureEventLinker<T>(eventName);

				if (!eventLinker.Observers.Add(observer))
					throw new InvalidOperationException("Observer already subscribed.");

				return
					Disposable.Create(
						() =>
						{
							using (_rwLock.GetWriterLock())
								eventLinker.Observers.Remove(observer);
							observer.OnCompleted();
						});
			}
		}

		#endregion

		#region Private Members

		[CanBeNull]
		private EventLinker GetEventLinker<T>(string eventName)
		{
			EventLinker eventLinker;
			if (!_events.TryGetValue(eventName, out eventLinker))
				return null;
			if (eventLinker.ArgumentType != typeof(T))
				throw new ApplicationException("Argument type is wrong.");
			return eventLinker;
		}

		[NotNull]
		private EventLinker EnsureEventLinker<T>(string eventName)
		{
			var eventLinker = GetEventLinker<T>(eventName);
			if (eventLinker == null)
			{
				eventLinker = new EventLinker(typeof(T));
				_events.Add(eventName, eventLinker);
			}
			return eventLinker;
		}

		#endregion

		#region EventLinker class
		private class EventLinker
		{
			public EventLinker(Type itemType)
			{
				ArgumentType = itemType;
			}

			public Type ArgumentType { get; }

			public HashSet<object> Observers { get; } = new HashSet<object>();
		}
		#endregion
	}
}