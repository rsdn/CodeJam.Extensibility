using NUnit.Framework;

namespace Rsdn.SmartApp.ActiveParts
{
	[TestFixture]
	public class ActivePartsTest
	{
		private ServiceManager _svcMgr;
		private ExtensionManager _extMgr;

		#region Setup/Teardown
		[SetUp]
		protected void SetUp()
		{
			_svcMgr = new ServiceManager();
			_extMgr = new ExtensionManager(_svcMgr);
			_extMgr.ReflectionOnlyScan(new ActivePartRegStrategy(_svcMgr), typeof(TestActivePart).Assembly.FullName);
			_svcMgr.Publish<IActivePartManager>(new ActivePartManager(_svcMgr));
		}
		#endregion

		[Test]
		public void InstanceCreationTest()
		{
			var svc = _svcMgr.GetRequiredService<IActivePartManager>();
			svc.Activate();
			Assert.IsNotNull(svc.GetPartInstance(typeof(TestActivePart)));
		}

		[Test]
		public void ActivatePassivate()
		{
			var svc = _svcMgr.GetRequiredService<IActivePartManager>();

			svc.Activate();
			var part = (TestActivePart)svc.GetPartInstance(typeof (TestActivePart));
			Assert.IsTrue(part.Activated, "#A01");

			svc.Passivate();
			Assert.IsFalse(part.Activated, "#A02");
		}

		[Test]
		public void Dispose()
		{
			var svc = _svcMgr.GetRequiredService<IActivePartManager>();

			svc.Activate();
			var part = (TestActivePart)svc.GetPartInstance(typeof(TestActivePart));
			Assert.IsFalse(part.Disposed, "#A01");

			_svcMgr.Dispose();
			Assert.IsTrue(part.Disposed, "#A02");
		}
	}
}