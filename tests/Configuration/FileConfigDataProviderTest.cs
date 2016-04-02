using System;
using System.IO;
using System.Reflection;
using System.Threading;

using NUnit.Framework;

namespace Rsdn.SmartApp.Configuration
{
	[TestFixture]
	public class FileConfigDataProviderTest
	{
		private static readonly string _exeDir =
			Path.GetDirectoryName(
				new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);

		[Test]
		public void ChangeNotification()
		{
			var fn = Path.Combine(_exeDir, "test.cfg");
			using (new StreamWriter(fn))
			{
			}
			var fcp = new FileConfigDataProvider(fn);
			var changeFired = false;
			var evt = new ManualResetEvent(false);
			fcp.ConfigChanged +=
				prov =>
					{
						changeFired = true;
						evt.Set();
					};
			Assert.IsFalse(changeFired, "#A01");
			using (new StreamWriter(fn))
			{
			}
			// Allow fire notification
			evt.WaitOne(5000, true); // No more that 5 seconds
			Assert.IsTrue(changeFired, "#A02");
		}

		[Test]
		public void FileNameEmpty()
		{
			// ReSharper disable once ObjectCreationAsStatement
			Assert.Throws<ArgumentNullException>(() => new FileConfigDataProvider(""));
		}

		[Test]
		public void FileNameNull()
		{
			// ReSharper disable once ObjectCreationAsStatement
			Assert.Throws<ArgumentNullException>(() => new FileConfigDataProvider(null));
		}

		[Test]
		public void FileNotExists()
		{
			// ReSharper disable once ObjectCreationAsStatement
			Assert.Throws<FileNotFoundException>(() =>new FileConfigDataProvider("X:/y/z"));
		}
	}
}