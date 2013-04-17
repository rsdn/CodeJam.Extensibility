using System;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

namespace Rsdn.SmartApp.Extensibility.Registration
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
			_svcManager = new ServiceManager();
			_extManager = new ExtensionManager(_svcManager);
		}
		#endregion

		private ServiceManager _svcManager;
		private ExtensionManager _extManager;

		private static void TestRegElementsInService<TInfo>(IRegElementsService<TInfo> svc)
			where TInfo : ElementInfo
		{
			var elems = svc.GetRegisteredElements();
			Assert.IsNotNull(elems);
			var sampleElem = elems.FirstOrDefault(elem => elem.ElementType == typeof (SampleElement));
			Assert.IsNotNull(sampleElem);
		}

		private void TestRegElements<TInfo>()
			where TInfo : ElementInfo
		{
			var svc = _extManager.GetService<IRegElementsService<TInfo>>();
			Assert.IsNotNull(svc);
			TestRegElementsInService(svc);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void NamedSvcContainsParamIsNull()
		{
			var svc = new NamedSvc();
			svc.ContainsElement(null);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void NamedSvcGetParamIsNull()
		{
			var svc = new NamedSvc();
			svc.GetElement(null);
		}

		[Test]
		public void RegElementsHelper()
		{
			_extManager.Scan(
				new ElementStrategy(_svcManager),
				Assembly.GetExecutingAssembly().GetTypes());
			TestRegElements<ElementInfo>();
		}

		[Test]
		public void RegElementsService()
		{
			var svc = new RegElementsService<ElementInfo>();
			svc.Register(new ElementInfo(typeof (SampleElement)));
			TestRegElementsInService(svc);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void RegElementsServiceInfoIsNull()
		{
			var svc = new RegElementsService<ElementInfo>();
			svc.Register(null);
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