using System;
using System.Reflection;

using NUnit.Framework;

namespace Rsdn.SmartApp.Services
{
	[TestFixture]
	public class ServiceManagerTest
	{
		#region Setup/Teardown
		[SetUp]
		protected void SetUp()
		{
			_serviceManager = new ServiceManager(true);
		}
		#endregion

		private const string _name = "TestName";
		private const string _name2 = "TestName2";

		private ServiceManager _serviceManager;

		private void AssertServiceReturned()
		{
			var svc = _serviceManager.GetService<ISampleService>();
			Assert.IsNotNull(svc, "Service not returned");
			Assert.AreEqual(svc.GetName(), _name);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void DuplicatePublishing()
		{
			_serviceManager.Publish(typeof (ISampleService), new SampleService(_name));
			_serviceManager.Publish(typeof (ISampleService), new SampleService(_name));
		}

		[Test]
		public void ImplementationOverriding()
		{
			_serviceManager.Publish(typeof(ISampleService), new SampleService(_name));
			var child = new ServiceManager(_serviceManager);
			child.Publish(typeof(ISampleService), new SampleService(_name2));
			var svc = child.GetService<ISampleService>();
			Assert.IsNotNull(svc);
			Assert.AreEqual(svc.GetName(), _name2);
		}

		[Test]
		public void ParentServiceProviderCall()
		{
			_serviceManager.Publish(typeof(ISampleService), new SampleService(_name));
			var child = new ServiceManager(_serviceManager);
			var svc = child.GetService<ISampleService>();
			Assert.IsNotNull(svc);
			Assert.AreEqual(svc.GetName(), _name);
		}

		[Test]
		public void PublishServiceInstance()
		{
			_serviceManager.Publish(typeof(ISampleService), new SampleService(_name));
			AssertServiceReturned();
		}

		[Test]
		public void PublishServiceWithCreator()
		{
			var svcCreated = false;
			ServiceCreator creator =
				(type, pub) =>
					{
						svcCreated = true;
						return new SampleService(_name);
					};
			_serviceManager.Publish(typeof(ISampleService), creator);
			Assert.IsFalse(svcCreated);
			AssertServiceReturned();
			Assert.IsTrue(svcCreated);
		}

		[Test]
		public void SelfPublishing()
		{
			Assert.AreEqual(
				_serviceManager.GetService<IServicePublisher>(),
				_serviceManager);
			var child = new ServiceManager(true, _serviceManager);
			Assert.AreEqual(child.GetService<IServicePublisher>(), child);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void ServiceCreatorNull()
		{
// ReSharper disable AssignNullToNotNullAttribute
			_serviceManager.Publish((ServiceCreator)null);
// ReSharper restore AssignNullToNotNullAttribute
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void ServiceCreatorReturnNull()
		{
			_serviceManager.Publish(typeof(ISampleService), (type, pub) => null);
			_serviceManager.GetService<ISampleService>();
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void ServiceInstanceNull()
		{
// ReSharper disable AssignNullToNotNullAttribute
			_serviceManager.Publish(typeof(ISampleService), (ISampleService)null);
// ReSharper restore AssignNullToNotNullAttribute
		}

		[Test]
		public void ServicePublishingHelper()
		{
			var extMgr = new ExtensionManager(_serviceManager);
			extMgr.Scan(
				ServicesHelper.CreateServiceStrategy(_serviceManager),
				Assembly.GetExecutingAssembly().GetTypes());
			Assert.IsNotNull(extMgr.GetService<ISampleService>());
		}

		[Test]
		public void Unpublishing()
		{
			var cookie = _serviceManager.Publish(
				typeof(ISampleService),
				new SampleService(_name));
			_serviceManager.Unpublish(cookie);
			Assert.IsNull(_serviceManager.GetService<ISampleService>());
		}

		[Test]
		public void DisposableService()
		{
			var svc = new DisposableService();

			using (var svcMgr = new ServiceManager())
			{
				svcMgr.Publish<IDisposableService>(svc);
				Assert.IsFalse(svc.Disposed, "#A01");
			}
			Assert.IsTrue(svc.Disposed, "#A02");
		}

		[Test]
		public void MultiContractSvc()
		{
			var extMgr = new ExtensionManager(_serviceManager);
			extMgr.Scan(
				ServicesHelper.CreateServiceStrategy(_serviceManager),
				Assembly.GetExecutingAssembly().GetTypes());

			var svc1 = _serviceManager.GetService<ISampleService2>();
			Assert.IsNotNull(svc1, "#A01");
			Assert.AreEqual(svc1, _serviceManager.GetService<ISampleService3>(), "#A02");
		}
	}
}