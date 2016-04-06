using System;
using System.Reflection;

using CodeJam.Services;

using NUnit.Framework;

namespace CodeJam.Extensibility
{
	[TestFixture]
	public class ExtensionManagerTest
	{
		#region Setup/Teardown
		[SetUp]
		protected void SetUp()
		{
			_svcManager = new ServiceContainer(true);
			_extManager = new ExtensionManager(_svcManager);
		}
		#endregion

		private ServiceContainer _svcManager;
		private ExtensionManager _extManager;

		[Test]
		public void AssemblyScanning()
		{
			_extManager.Scan(
				new SimpleExtensionStrategy(),
				Assembly.GetExecutingAssembly().GetTypes());
			Assert.AreEqual(
				typeof (SimpleExtension).AssemblyQualifiedName,
				SimpleExtensionStrategy.LastExtensionTypeName);
		}

		[Test]
		public void NullStrategy()
		{
			// ReSharper disable once AssignNullToNotNullAttribute
			Assert.Throws<ArgumentNullException>(() => _extManager.Scan(null, new Type[0]));
		}

		[Test]
		public void NullTypes()
		{
			Assert.Throws<ArgumentNullException>(() => _extManager.Scan(null, new Assembly[0]));
		}

		[Test]
		public void SelfPublishing()
		{
			Assert.AreEqual(_extManager.GetService<IExtensionManager>(), _extManager);
		}

		[Test]
		public void ReflectionLoad()
		{
			_extManager.ReflectionOnlyScan(
				new SimpleExtensionStrategy(),
				Assembly.GetExecutingAssembly().FullName);
			Assert.AreEqual(
				typeof(SimpleExtension).AssemblyQualifiedName,
				SimpleExtensionStrategy.LastExtensionTypeName);
		}

		[Test]
		public void ReflectionLoadFrom()
		{
			_extManager.ReflectionOnlyFromScan(
				new SimpleExtensionStrategy(),
				Assembly.GetExecutingAssembly().CodeBase);
			Assert.AreEqual(
				typeof(SimpleExtension).AssemblyQualifiedName,
				SimpleExtensionStrategy.LastExtensionTypeName);
		}

		[Test]
		public void ExtensionCache()
		{
			var root = new ServiceContainer(true);
			var strategy =
				root
					.CreateStrategy<SimpleExtensionInfo, SimpleExtensionAttribute>(
						(ctx, attr) => new SimpleExtensionInfo(ctx.Type));
			_extManager.ReflectionOnlyScanExtensions(
				new[]{Assembly.GetExecutingAssembly().FullName},
				strategy);
			var cache = new ExtensionsCache<SimpleExtensionInfo, SimpleExtension>(root);
			var exts = cache.GetAllExtensions();
			Assert.IsNotNull(exts);
			Assert.AreEqual(1, exts.Length);
			Assert.IsNotNull(exts[0]);
		}

		[Test]
		public void AttrProps()
		{
			var root = new ServiceContainer(true);
			var propValue = false;
			var strategy =
				root
					.CreateStrategy<SimpleExtensionInfo, SimpleExtensionAttribute>(
						(ctx, attr) =>
						{
							propValue = attr.Prop;
							return new SimpleExtensionInfo(ctx.Type);
						});
			_extManager.ReflectionOnlyScanExtensions(
				new[] { Assembly.GetExecutingAssembly().FullName },
				strategy);
			Assert.IsTrue(propValue);
		}
	}
}