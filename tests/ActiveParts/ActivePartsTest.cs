using CodeJam.Services;

using NUnit.Framework;

namespace CodeJam.Extensibility.ActiveParts
{
	[TestFixture]
	public class ActivePartsTest
	{
		private ServiceContainer _svcContainer;
		private ExtensionManager _extMgr;

		#region Setup/Teardown
		[SetUp]
		protected void SetUp()
		{
			_svcContainer = new ServiceContainer();
			_extMgr = new ExtensionManager(_svcContainer);
			_extMgr.ReflectionOnlyScan(new ActivePartRegStrategy(_svcContainer), typeof(TestActivePart).Assembly.FullName);
			_svcContainer.Publish<IActivePartManager>(sp => new ActivePartManager(_svcContainer));
		}
		#endregion

		[Test]
		public void InstanceCreationTest()
		{
			var svc = _svcContainer.GetRequiredService<IActivePartManager>();
			svc.Activate();
			Assert.IsNotNull(svc.GetPartInstance(typeof(TestActivePart)));
		}

		[Test]
		public void ActivatePassivate()
		{
			var svc = _svcContainer.GetRequiredService<IActivePartManager>();

			svc.Activate();
			var part = (TestActivePart)svc.GetPartInstance(typeof (TestActivePart));
			Assert.IsTrue(part.Activated, "#A01");

			svc.Passivate();
			Assert.IsFalse(part.Activated, "#A02");
		}

		[Test]
		public void Dispose()
		{
			var svc = _svcContainer.GetRequiredService<IActivePartManager>();

			svc.Activate();
			var part = (TestActivePart)svc.GetPartInstance(typeof(TestActivePart));
			Assert.IsFalse(part.Disposed, "#A01");

			_svcContainer.Dispose();
			Assert.IsTrue(part.Disposed, "#A02");
		}
	}
}