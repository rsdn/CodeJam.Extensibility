namespace Rsdn.SmartApp.CommandLine
{
	internal class CmdLineSvcMock : CommandLineExtensibleService
	{
		private readonly string _commandLineText;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CmdLineSvcMock(string commandLineText)
			: base(
				new ServiceManager(),
				() => new[] { new CommandLineRulesProviderInfo(typeof(CmdLineProviderMock)) },
				CommandQuantifier.ZeroOrOne)
		{
			_commandLineText = commandLineText;
		}

		protected override string GetCommandLine()
		{
			return _commandLineText;
		}
	}
}