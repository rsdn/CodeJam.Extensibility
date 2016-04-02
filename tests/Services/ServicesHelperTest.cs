using NUnit.Framework;

namespace CodeJam.Extensibility.Services
{
	[TestFixture]
	public class ServicesHelperTest
	{
		#region Setup/Teardown
		[SetUp]
		protected void SetUp()
		{
			_serviceManager = new ServiceManager();
		}
		#endregion

		private ServiceManager _serviceManager;

		[Test]
		public void AssignNonExistentRequiredService()
		{
			var su = new ServiceUser();
			Assert.Throws<ServiceNotFoundException>(() => su.AssignServices(_serviceManager));
		}

		[Test]
		public void AssignServices()
		{
			var ss = new SampleService();
			_serviceManager.Publish<ISampleService>(ss);
			var su = new ServiceUser();
			su.AssignServices(_serviceManager);

			Assert.AreEqual(ss, su.Svc1);
			Assert.AreEqual(ss, su.Svc2);
			Assert.AreEqual(ss, su.Svc3);
		}

		[Test]
		public void GetService()
		{
			_serviceManager.Publish<ISampleService>(new SampleService());
			Assert.IsNotNull(_serviceManager.GetService(typeof (ISampleService)));
		}

		[Test]
		public void PublishCreator()
		{
			_serviceManager.Publish<ISampleService>(pub => new SampleService());
			Assert.IsNotNull(_serviceManager.GetService<ISampleService>());
		}

		[Test]
		public void PublishDisposableCreator()
		{
			using (_serviceManager.PublishDisposable<ISampleService>(pub => new SampleService()))
				Assert.IsNotNull(_serviceManager.GetService<ISampleService>());
			Assert.IsNull(_serviceManager.GetService<ISampleService>());
		}

		[Test]
		public void PublishInstance()
		{
			_serviceManager.Publish<ISampleService>(new SampleService());
			Assert.IsNotNull(_serviceManager.GetService<ISampleService>());
		}

		[Test]
		public void PublishDisposableInstance()
		{
			using (_serviceManager.PublishDisposable<ISampleService>(new SampleService()))
				Assert.IsNotNull(_serviceManager.GetService<ISampleService>());
			Assert.IsNull(_serviceManager.GetService<ISampleService>());
		}
	}
}