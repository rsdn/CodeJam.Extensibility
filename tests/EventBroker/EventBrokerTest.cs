using CodeJam.Services;

using NUnit.Framework;

namespace CodeJam.Extensibility.EventBroker
{
	[TestFixture]
	public class EventBrokerTest
	{
		[Test]
		public void RegisterSubscribeTest()
		{
			var eb = new EventBroker();
			var i = 0;
			const string eventName = "TestEvent";
			using (eb.Subscribe(eventName, System.Reactive.Observer.Create<int>(item => i += item)))
			{
				eb.Fire(eventName,1);
				Assert.AreEqual(1, i);

				using (eb.Subscribe(eventName, System.Reactive.Observer.Create<int>(item => i += item)))
				{
					eb.Fire(eventName, 1);
					Assert.AreEqual(3, i);
				}
			}
		}

		[Test]
		public void MappingTest()
		{
			using (var serviceManager = new ServiceContainer())
			{
				serviceManager.Publish<IEventBroker>(new EventBroker());

				using (var testObject = new MappingTestObject(serviceManager))
				{
					testObject.FireTestEvent(1);
					Assert.AreEqual(1, testObject.TestValue);

					testObject.FireTestObservable(1);
					Assert.AreEqual(2, testObject.TestValue);
				}
			}
		}
	}
}