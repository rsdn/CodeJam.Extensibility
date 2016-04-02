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
			public string Message
			{
				get { return "TestSvc"; }
			}
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
		[ExpectedException(typeof (ArgumentException))]
		public void NoCustomParam()
		{
			InstancingHelper.CreateInstance<CustomParamsCtor>(_svcMgr);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void WrongCustomParam()
		{
			InstancingHelper.CreateInstance<MarkedDefaultCtor>(
				_svcMgr,
				new InstancingCustomParam("message", "", false));
		}

		[Test]
		public void OptionalCustomParam()
		{
			InstancingHelper.CreateInstance<MarkedDefaultCtor>(
				_svcMgr,
				new InstancingCustomParam("message", "", true));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ReqCustomParamAbsent()
		{
			InstancingHelper.CreateInstance<CustomParamsCtor>(
				_svcMgr,
				new InstancingCustomParam("messag", ""));
		}

		[Test]
		public void DefaultCtorMarkup()
		{
			Assert.AreEqual("default",
				InstancingHelper.CreateInstance<MarkedDefaultCtor>(_svcMgr).Message);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void MultipleDefaultCtors()
		{
			InstancingHelper.CreateInstance<MultipleDefaultCtors>(_svcMgr);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void NoDefaultCtor()
		{
			InstancingHelper.CreateInstance<NoDefaultCtor>(_svcMgr);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void NoPublicCtors()
		{
			InstancingHelper.CreateInstance<NoPublicCtors>(_svcMgr);
		}

		[Test]
		public void OnlyProviderParam()
		{
			Assert.AreEqual(_svcMgr, InstancingHelper.CreateInstance<OnlyProviderParam>(_svcMgr).Provider);
		}
	}
}