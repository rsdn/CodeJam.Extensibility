namespace Rsdn.SmartApp.CommandLine
{
	/// <summary>
	/// Command line service contract.
	/// </summary>
	public interface ICommandLineService
	{
		/// <summary>
		/// Returns file name, that used to execute program.
		/// </summary>
		string GetProgramName();

		/// <summary>
		/// Return all specified commands.
		/// </summary>
		string[] GetCommands();

		/// <summary>
		/// Returns true, if specified command defined.
		/// </summary>
		bool IsCommandDefined(string commandName);

		/// <summary>
		/// Return all specified options.
		/// </summary>
		string[] GetOptions();

		/// <summary>
		/// Returns true, if specified option defined.
		/// </summary>
		bool IsOptionDefined(string optionName);

		/// <summary>
		/// Return value of bool option.
		/// </summary>
		bool GetBoolOptionValue(string option);

		/// <summary>
		/// Return value of value option.
		/// </summary>
		string GetValueOptionValue(string option);
	}
}