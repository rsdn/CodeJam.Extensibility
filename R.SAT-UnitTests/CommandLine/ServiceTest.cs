using NUnit.Framework;

namespace Rsdn.SmartApp.CommandLine
{
	[TestFixture]
	public class ServiceTest
	{
		[Test]
		public void MainTest()
		{
			ICommandLineService svc = new CmdLineSvcMock("program.exe cmd /opt1 /opt2+ /opt3=\"O La La\"");

			Assert.AreEqual("program.exe", svc.GetProgramName(), "#A01");

			Assert.AreEqual(new[] { "cmd" }, svc.GetCommands(), "#A02");
			Assert.IsTrue(svc.IsCommandDefined("cmd"), "#A03");

			Assert.AreEqual(new[] { "opt1", "opt2", "opt3" }, svc.GetOptions(), "#A04");
			Assert.IsTrue(svc.IsOptionDefined("opt1"), "#A05");
			Assert.IsTrue(svc.IsOptionDefined("opt2"), "#A06");
			Assert.IsTrue(svc.IsOptionDefined("opt3"), "#A07");
			Assert.IsTrue(svc.GetBoolOptionValue("opt2"), "#A08");
			Assert.AreEqual("O La La", svc.GetValueOptionValue("opt3"), "#A09");
		}
	}
}