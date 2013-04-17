using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Schema;

using NUnit.Framework;

namespace Rsdn.SmartApp.Configuration
{
	[TestFixture]
	public class ConfigServiceTest
	{
		private static readonly string _exeDir =
			Path.GetDirectoryName(
				new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);

		[Test]
		public void NormalRun()
		{
			var svc = new ConfigService(
				new[] { typeof(ISection1), typeof(ISection2), typeof(ISection3) }
					.GetSectionInfos(),
				Path.Combine(_exeDir, "Configuration/testconfig.xml"),
				null);

			var sec1 = svc.GetSection<ISection1>();
			Assert.IsNotNull(sec1);
			Assert.AreEqual("Sample text", sec1.Text);

			var sec2 = svc.GetSection<ISection2>();
			Assert.IsNotNull(sec2);
			Assert.AreEqual(21, sec2.Number);

			var sec3 = svc.GetSection<ISection3>();
			Assert.IsNotNull(sec3);
			Assert.AreEqual(2, sec3.Values.Length);
			Assert.AreEqual("Value 1", sec3.Values[0]);
			Assert.AreEqual("Value 2", sec3.Values[1]);
		}

		[Test]
		public void SchemaValidationFail()
		{
			var svc = new ConfigService(
				new[] { typeof(ISection1) }.GetSectionInfos(),
				Path.Combine(_exeDir, "Configuration/testconfig-invalid.xml"),
				new Dictionary<string, string>());
			try
			{
				svc.GetSection<ISection1>();
			}
			catch (InvalidOperationException ex)
			{
				Assert.IsTrue(ex.InnerException is XmlSchemaValidationException);
				return;
			}
			catch (XmlSchemaValidationException)
			{
				return;
			}
			Assert.Fail();
		}

		[Test]
		public void VarsTest()
		{
			var svc = new ConfigService(
				new[] { typeof(ISection4) }.GetSectionInfos(),
				Path.Combine(_exeDir, "Configuration/testvars.xml"),
				new Dictionary<string, string> {{"Var2", "Value2"}});
			Assert.IsNotNull(svc, "svc");
			var sec4 = svc.GetSection<ISection4>();
			Assert.IsNotNull(sec4, "sec4");
			Assert.AreEqual("PrefixValue1Postfix", sec4.Name, "SectionName");
			Assert.AreEqual("PrefixValue2PostfixValue12", sec4.Value.Trim(), "SectionValue");
		}
	}
}