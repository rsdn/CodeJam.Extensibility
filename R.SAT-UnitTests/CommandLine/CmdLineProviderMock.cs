using JetBrains.Annotations;

namespace Rsdn.SmartApp.CommandLine
{
	[UsedImplicitly]
	internal class CmdLineProviderMock : ICommandLineRulesProvider
	{
		#region Implementation of ICommandLineRulesProvider
		public CommandRule[] GetCommands()
		{
			return new[] { new CommandRule("cmd") };
		}

		public OptionRule[] GetOptions()
		{
			return new []
				{
					new OptionRule("opt1"),
					new OptionRule("opt2", OptionType.Bool),
					new OptionRule("opt3", OptionType.Value),
				};
		}
		#endregion
	}
}