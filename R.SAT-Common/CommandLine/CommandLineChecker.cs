 using System;
 using System.Collections.Generic;
 using System.ComponentModel;
 using System.Linq;

namespace Rsdn.SmartApp.CommandLine
{
	/// <summary>
	/// Methods for checking command line semantics.
	/// </summary>
	[Localizable(false)]
	internal static class CommandLineChecker
	{
		/// <summary>
		/// Check command line semantics.
		/// </summary>
		public static void Check(CmdLineNode commandLine, CmdLineRules rules)
		{
			// Check rules consistency
			var cmds = new HashSet<string>();
			foreach (var command in rules.Commands)
				if (!cmds.Add(command.Name))
					throw new CommandLineCheckException("Duplicate commands '{0}'".FormatStr(command.Name));
			var cmdOpts = new ElementsCache<string, HashSet<string>>(cn => new HashSet<string>());
			foreach (var cmdOpt in 
				rules
					.Options
					.SelectMany(
						opt =>
							!opt.DependOnCommands.Any()
								? new[] { new {Cmd = "", Opt = opt.Name} }
								: opt.DependOnCommands.Select(cn => new {Cmd = cn, Opt = opt.Name})))
			{
				if (!cmdOpts.Get(cmdOpt.Cmd).Add(cmdOpt.Opt))
					throw new CommandLineCheckException(
						"Duplicate option {0}{1}"
							.FormatStr(
								cmdOpt.Opt,
								cmdOpt.Cmd == ""
									? ""
									: " in command {0}".FormatStr(cmdOpt.Cmd)));
			}

			// Check command quantity
			var cmdCount = commandLine.Commands.Length;
			switch (rules.CommandQuantifier)
			{
				case CommandQuantifier.ZeroOrOne:
					if (cmdCount > 1)
						throw new CommandLineCheckException("Maximum one command is allowed.");
					break;
				case CommandQuantifier.ZeroOrMultiple:
					// Any combination allowed
					break;
				case CommandQuantifier.One:
					if (cmdCount > 1)
						throw new CommandLineCheckException("Maximum one command is allowed.");
					if (cmdCount == 0)
						throw new CommandLineCheckException("Required command not specified.");
					break;
				case CommandQuantifier.OneOrMultiple:
					if (cmdCount == 0)
						throw new CommandLineCheckException("Required command(s) not specified.");
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			// Check for unknown commands
			var cmdRules = rules.Commands.ToDictionary(cr => cr.Name);
			var unkCmds =
				commandLine
					.Commands
					.Where(cmd => !cmdRules.ContainsKey(cmd.Text))
					.ToArray();
			if (unkCmds.Length > 0)
				throw new CommandLineCheckException(
					"Unknown commands :{0}".FormatStr(unkCmds.Select(cmd => "'" + cmd.Text + "'").Join(", ")));

			// Check for unknown options
			var optRules = rules.Options.ToDictionary(cr => cr.Name);
			var unkOpts =
				commandLine
					.Options
					.Where(opt => !optRules.ContainsKey(opt.Text))
					.ToArray();
			if (unkOpts.Length > 0)
				throw new CommandLineCheckException(
					"Unknown options :{0}".FormatStr(unkOpts.Select(opt => "'" + opt.Text + "'").Join(", ")));

			// Option values check
			foreach (var option in commandLine.Options)
			{
				var rule = optRules[option.Text];
				if (rule.Type != option.Type)
					switch (rule.Type)
					{
						case OptionType.Valueless:
							throw new CommandLineCheckException("Option '{0}' cannot have value".FormatStr(rule.Name));
						case OptionType.Bool:
							throw new CommandLineCheckException("'+' or '-' must be specified for option '{0}'".FormatStr(rule.Name));
						case OptionType.Value:
							throw new CommandLineCheckException("Value must be specified for option '{0}'".FormatStr(rule.Name));
						default:
							throw new ArgumentOutOfRangeException();
					}
			}

			// Options presence check
			var specCmds = new HashSet<string>(commandLine.Commands.Select(cmd => cmd.Text));
			var specOpts = new HashSet<string>(commandLine.Options.Select(opt => opt.Text));
			foreach (var optRule in rules.Options)
			{
				var existed = specOpts.Contains(optRule.Name);
				var dependent = optRule.DependOnCommands.Any();
				var hasMasterCmd = dependent && optRule.DependOnCommands.Any(specCmds.Contains);
				if (existed && dependent && !hasMasterCmd)
					throw new CommandLineCheckException(
						"{0} command(s) must be specified for use {1} option."
							.FormatStr(optRule.DependOnCommands.FormatStr("'{0}'").Join(", "), optRule.Name));

				if (!existed && optRule.Required && (!dependent || hasMasterCmd))
					throw new CommandLineCheckException("Required option '{0}' absent.".FormatStr(optRule.Name));
			}
		}
	}
}