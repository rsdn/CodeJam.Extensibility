using System;

using NUnit.Framework;

namespace Rsdn.SmartApp.Instancing
{
	[TestFixture]
	public class InstancingHelperTest
	{
		#region Setup/Teardown
		[SetUp]
		protected void SetUp()
		{
			_svcMgr = new ServiceManager();
			_svcMgr.Publish<ITestSvc>(new TestSvc());
		}
		#endregion

		private ServiceManager _svcMgr;

		private class TestSvc : ITestSvc
		{
			#region ITestSvc Members
			public string Message => "TestSvc";
			#endregion
		}

		[Test]
		public void CustomParams()
		{
			const string testMessage = "TestMessage";
			var c = InstancingHelper.CreateInstance<CustomParamsCtor>(
				_svcMgr,
				new InstancingCustomParam("message", testMessage));
			Assert.AreEqual(testMessage, c.Message);
		}

		[Test]
		public void NoCustomParam()
		{
			Assert.Throws<ArgumentException>(() => InstancingHelper.CreateInstance<CustomParamsCtor>(_svcMgr));
		}

		[Test]
		public void WrongCustomParam()
		{
			Assert.Throws<ArgumentException>(() => 
				InstancingHelper.CreateInstance<MarkedDefaultCtor>(
					_svcMgr,
					new InstancingCustomParam("message", "", false)));
		}

		[Test]
		public void OptionalCustomParam()
		{
			InstancingHelper.CreateInstance<MarkedDefaultCtor>(
				_svcMgr,
				new InstancingCustomParam("message", "", true));
		}

		[Test]
		public void ReqCustomParamAbsent()
		{
			Assert.Throws<ArgumentException>(() =>
				InstancingHelper.CreateInstance<CustomParamsCtor>(
					_svcMgr,
					new InstancingCustomParam("messag", "")));
		}

		[Test]
		public void DefaultCtorMarkup()
		{
			Assert.AreEqual("default",
				InstancingHelper.CreateInstance<MarkedDefaultCtor>(_svcMgr).Message);
		}

		[Test]
		public void MultipleDefaultCtors()
		{
			Assert.Throws<ArgumentException>(() => InstancingHelper.CreateInstance<MultipleDefaultCtors>(_svcMgr));
		}

		[Test]
		public void NoDefaultCtor()
		{
			Assert.Throws<ArgumentException>(() => InstancingHelper.CreateInstance<NoDefaultCtor>(_svcMgr));
		}

		[Test]
		public void NoPublicCtors()
		{
			Assert.Throws<ArgumentException>(() => InstancingHelper.CreateInstance<NoPublicCtors>(_svcMgr));
		}

		[Test]
		public void OnlyProviderParam()
		{
			Assert.AreEqual(_svcMgr, InstancingHelper.CreateInstance<OnlyProviderParam>(_svcMgr).Provider);
		}
	}
}