using System;
using System.Linq;
using System.Reflection;

using CodeJam.Extensibility.Registration;
using CodeJam.Services;

using NUnit.Framework;

namespace CodeJam.Extensibility
{
	using INamedSvc = IRegKeyedElementsService<string, TestNamedElementInfo>;
	using NamedSvc = RegKeyedElementsService<string, TestNamedElementInfo>;

	[TestFixture]
	public class RegistrationTest
	{
		#region Setup/Teardown
		[SetUp]
		protected void SetUp()
		{
			_svcManager = new ServiceContainer();
			_extManager = new ExtensionManager(_svcManager);
		}
		#endregion

		private ServiceContainer _svcManager;
		private ExtensionManager _extManager;

		private static void TestRegElementsInService<TInfo>(IRegElementsService<TInfo> svc)
			where TInfo : TestElementInfo
		{
			var elems = svc.GetRegisteredElements();
			Assert.IsNotNull(elems);
			var sampleElem = elems.FirstOrDefault(elem => elem.ElementType == typeof (SampleElement));
			Assert.IsNotNull(sampleElem);
		}

		private void TestRegElements<TInfo>()
			where TInfo : TestElementInfo
		{
			var svc = _extManager.GetService<IRegElementsService<TInfo>>();
			Assert.IsNotNull(svc);
			TestRegElementsInService(svc);
		}

		[Test]
		public void NamedSvcContainsParamIsNull()
		{
			var svc = new NamedSvc();
			Assert.Throws<ArgumentNullException>(() => svc.ContainsElement(null));
		}

		[Test]
		public void NamedSvcGetParamIsNull()
		{
			var svc = new NamedSvc();
			Assert.Throws<ArgumentNullException>(() => svc.GetElement(null));
		}

		[Test]
		public void RegElementsHelper()
		{
			_extManager.Scan(
				new ElementStrategy(_svcManager),
				Assembly.GetExecutingAssembly().GetTypes());
			TestRegElements<TestElementInfo>();
		}

		[Test]
		public void RegElementsService()
		{
			var svc = new RegElementsService<TestElementInfo>();
			svc.Register(new TestElementInfo(typeof (SampleElement)));
			TestRegElementsInService(svc);
		}

		[Test]
		public void RegElementsServiceInfoIsNull()
		{
			var svc = new RegElementsService<TestElementInfo>();
			Assert.Throws<ArgumentNullException>(() => svc.Register(null));
		}

		[Test]
		public void RegNamedElementsHelper()
		{
			_extManager.Scan(
				new NamedElementStrategy(_svcManager),
				Assembly.GetExecutingAssembly().GetTypes());
			// Test old service inherited functionality
			TestRegElements<TestNamedElementInfo>();
			var svc = _extManager.GetRequiredService<INamedSvc>();
			// Test new service inherited functionality
			TestRegElementsInService(svc);
			// Test own functionality
			Assert.IsFalse(svc.ContainsElement(""));
			Assert.IsTrue(svc.ContainsElement(SampleElement.Name));
			Assert.AreEqual(svc.GetElement(SampleElement.Name).ElementType,
				typeof (SampleElement));
		}
	}
}