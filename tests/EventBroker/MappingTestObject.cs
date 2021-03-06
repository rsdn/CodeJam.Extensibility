using System;
using System.Reactive.Subjects;

using JetBrains.Annotations;

namespace CodeJam.Extensibility.EventBroker
{
	public delegate void TestDelegate(int arg);

	public class MappingTestObject : IDisposable
	{
		const string _eventName = "TestEvent";
		private readonly Subject<int> _observable = new Subject<int>();
		private readonly IDisposable _eventSourcesRegistration;
		private readonly IDisposable _eventHandlersSubscription;

		public int TestValue { get; private set; }

		public MappingTestObject([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			_eventSourcesRegistration = EventBrokerHelper.RegisterEventSources(this, serviceProvider);
			_eventHandlersSubscription = EventBrokerHelper.SubscribeEventHandlers(this, serviceProvider);
		}

		[EventSource(_eventName)]
		public event TestDelegate TestEvent;

		public void FireTestEvent(int item)
		{
			TestEvent?.Invoke(item);
		}

		[EventSource(_eventName)]
		public IObservable<int> TestObservable => _observable;

		public void FireTestObservable(int item)
		{
			_observable.OnNext(item);
		}

		[EventHandler(_eventName)]
		private void TestHandler(int item)
		{
			TestValue += item;
		}

		#region Implementation of IDisposable

		public void Dispose()
		{
			_eventSourcesRegistration.Dispose();
			_eventHandlersSubscription.Dispose();
			_observable.OnCompleted();
		}

		#endregion
	}
}