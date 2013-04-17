using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp.CommandLine
{
	/// <summary>
	/// Command line rules.
	/// </summary>
	public class CmdLineRules
	{
		private readonly CommandQuantifier _commandQuantifier;
		private readonly CommandRule[] _commands;
		private readonly OptionRule[] _options;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CmdLineRules([NotNull] params CommandRule[] commands) : this(commands, new OptionRule[0])
		{}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CmdLineRules([NotNull] params OptionRule[] options) : this(new CommandRule[0], options)
		{}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CmdLineRules(
			[NotNull] CommandRule[] commands,
			[NotNull] OptionRule[] options) : this(CommandQuantifier.ZeroOrMultiple, commands, options)
		{}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CmdLineRules(
			CommandQuantifier commandQuantifier,
			[NotNull] CommandRule[] commands)
			: this(commandQuantifier, commands, new OptionRule[0])
		{}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CmdLineRules(
			CommandQuantifier commandQuantifier,
			[NotNull] CommandRule[] commands,
			[NotNull] OptionRule[] options)
		{
			if (commands == null)
				throw new ArgumentNullException("commands");
			if (options == null)
				throw new ArgumentNullException("options");
			_commandQuantifier = commandQuantifier;
			_commands = commands;
			_options = options;
		}

		/// <summary>
		/// Command quantifier.
		/// </summary>
		public CommandQuantifier CommandQuantifier
		{
			get { return _commandQuantifier; }
		}

		/// <summary>
		/// Commands.
		/// </summary>
		public CommandRule[] Commands
		{
			get { return _commands; }
		}

		/// <summary>
		/// Options.
		/// </summary>
		public OptionRule[] Options
		{
			get { return _options; }
		}
	}
}