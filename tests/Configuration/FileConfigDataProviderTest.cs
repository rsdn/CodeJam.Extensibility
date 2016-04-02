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
		[ExpectedException(typeof (ArgumentNullException))]
		public void FileNameEmpty()
		{
			new FileConfigDataProvider("");
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void FileNameNull()
		{
			new FileConfigDataProvider(null);
		}

		[Test]
		[ExpectedException(typeof (FileNotFoundException))]
		public void FileNotExists()
		{
			new FileConfigDataProvider("X:/y/z");
		}
	}
}