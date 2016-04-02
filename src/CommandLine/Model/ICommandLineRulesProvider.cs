namespace CodeJam.Extensibility.CommandLine
{
	/// <summary>
	/// Command line rules provider.
	/// </summary>
	public interface ICommandLineRulesProvider
	{
		/// <summary>
		/// Returns command rules.
		/// </summary>
		CommandRule[] GetCommands();

		/// <summary>
		/// Returns option rules.
		/// </summary>
		OptionRule[] GetOptions();
	}
}