using System;
using System.Linq;

using JetBrains.Annotations;

namespace Rsdn.SmartApp.CommandLine
{
	/// <summary>
	/// Implementation of <see cref="ICommandLineService"/>
	/// </summary>
	public class CommandLineService : ICommandLineService
	{
		private readonly Func<CmdLineRules> _rulesGetter;
		private readonly object _initLockFlag = new object();
		private bool _inited;
		private CmdLineNode _parsedCmdLine;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CommandLineService([NotNull] Func<CmdLineRules> rulesGetter)
		{
			if (rulesGetter == null) throw new ArgumentNullException("rulesGetter");

			_rulesGetter = rulesGetter;
		}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CommandLineService([NotNull] CmdLineRules rules) : this(() => rules)
		{}

		private void EnsureInit()
		{
			if (!_inited)
				lock (_initLockFlag)
					if (!_inited)
					{
						Init();
						_inited = true;
					}
		}

		private void Init()
		{
			var ast = CommandLineHelper.ParseCommandLine(GetCommandLine());
			CommandLineHelper.Check(ast, _rulesGetter());
			_parsedCmdLine = ast;
		}

		/// <summary>
		/// Returns command line.
		/// </summary>
		protected virtual string GetCommandLine()
		{
			return Environment.CommandLine;
		}

		#region Implementation of ICommandLineService
		/// <summary>
		/// Returns program file name.
		/// </summary>
		public string GetProgramName()
		{
			EnsureInit();
			return _parsedCmdLine.ProgramName.Text;
		}

		/// <summary>
		/// Return all used command names.
		/// </summary>
		public string[] GetCommands()
		{
			EnsureInit();
			return _parsedCmdLine.Commands.Select(cmd => cmd.Text).ToArray();
		}

		/// <summary>
		/// Returns true, if supplied command was used.
		/// </summary>
		public bool IsCommandDefined(string commandName)
		{
			EnsureInit();
			return _parsedCmdLine.Commands.Any(cmd => cmd.Text == commandName);
		}

		/// <summary>
		/// Returns all used option names.
		/// </summary>
		public string[] GetOptions()
		{
			EnsureInit();
			return _parsedCmdLine.Options.Select(opt => opt.Text).ToArray();
		}

		/// <summary>
		/// Returns true, if supplied option was used.
		/// </summary>
		public bool IsOptionDefined(string optionName)
		{
			EnsureInit();
			return _parsedCmdLine.Options.Any(opt => opt.Text == optionName);
		}

		/// <summary>
		/// Returns boolean option value.
		/// </summary>
		public bool GetBoolOptionValue(string optionName)
		{
			EnsureInit();
			var option = _parsedCmdLine.Options.FirstOrDefault(opt => opt.Text == optionName);
			if (option == null)
				throw new ArgumentException("Option not specified");
			if (option.Type != OptionType.Bool)
				throw new ArgumentException("Option is not bool");
			return option.BoolValue;
		}

		/// <summary>
		/// Returns value option value.
		/// </summary>
		public string GetValueOptionValue(string optionName)
		{
			EnsureInit();
			var option = _parsedCmdLine.Options.FirstOrDefault(opt => opt.Text == optionName);
			if (option == null)
				throw new ArgumentException("Option not specified");
			if (option.Type != OptionType.Value)
				throw new ArgumentException("Option is not value");
			return option.Value.Text;
		}
		#endregion
	}
}