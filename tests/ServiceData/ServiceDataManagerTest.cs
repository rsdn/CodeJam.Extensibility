using System;
using System.IO;

using NUnit.Framework;

namespace Rsdn.SmartApp.ServiceData
{
	[TestFixture]
	public class ServiceDataManagerTest
	{
		private const string _dataFolder = "C:/Temp";

		private ServiceDataManager _manager;

		[SetUp]
		protected void SetUp()
		{
			_manager = new ServiceDataManager(_dataFolder);
			var fileName = _manager.GetDataFileName(typeof(ISimpleDataMock));
			if (File.Exists(fileName))
				File.Delete(fileName);
		}

		[Test]
		[ExpectedException(typeof (ArgumentException))]
		public void BadType()
		{
			_manager.GetServiceDataInstance<string>();
		}

		[Test]
		public void InstancePropertyImpl()
		{
			var data = _manager.GetServiceDataInstance<ISimpleDataMock>();

			var ei = data.IntProp;
			Assert.AreEqual(0, ei, "#A05");

			const int i = 123;
			data.IntProp = i;
			Assert.AreEqual(i, data.IntProp, "#A06");

			object evtData = null;
			var evtPropertyName = "";
			IServiceDataManager evtManager = null;
			Type evtType = null;
			_manager.PropertyChanged +=
				(mgr, type, inst, name) =>
					{
						evtManager = mgr;
						evtType = type;
						evtData = inst;
						evtPropertyName = name;
					};
			const string str = "Test String";
			data.StringProp = str;
			Assert.AreEqual(str, data.StringProp, "#A01");
			Assert.AreEqual(_manager, evtManager, "#A02");
			Assert.AreEqual(data, evtData, "#A03");
			Assert.AreEqual("StringProp", evtPropertyName, "#A04");
			Assert.AreEqual(typeof (ISimpleDataMock), evtType, "#A07");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetAfterReset()
		{
			var data = _manager.GetServiceDataInstance<ISimpleDataMock>();
			_manager.ResetCache();
			var str = data.StringProp;
		}

		[Test]
		[ExpectedException(typeof (InvalidOperationException))]
		public void SetAfterReset()
		{
			var data = _manager.GetServiceDataInstance<ISimpleDataMock>();
			_manager.ResetCache();
			data.StringProp = "";
		}

		[Test]
		public void DataPersistenceTest()
		{
			var data = _manager.GetServiceDataInstance<ISimpleDataMock>();
			const string str = "Test String";
			data.StringProp = str;
			_manager.ResetCache();
			data = _manager.GetServiceDataInstance<ISimpleDataMock>();
			Assert.AreEqual(str, data.StringProp);
		}
	}
}
