using System.Collections.Generic;
using System.ComponentModel;

namespace Rsdn.SmartApp.CommandLine
{
	///<summary>
	/// Command line parser.
	///</summary>
	///<remarks>
	/// Grammar:
	///   CmdLine = ProgramName {Command | Option}
	///   ProgramName = QuotedOrUnquotedValue
	///   Command = NonWsChar {NonWsChar}
	///   Option = ('/' | '-') NonWsChar {NonWsChar} [OptionValue]
	///   OptionValue = ('+' | '-') | ('=' QuotedOrUnquotedValue)
	///</remarks>
	[Localizable(false)]
	internal static partial class CommandLineParser
	{
		/// <summary>
		/// Quota char.
		/// </summary>
		private const char _quota = '"';

		private static ParseResult<T> CreateResult<T>(T result, ICharInput inputRest)
		{
			return new ParseResult<T>(result, inputRest);
		}

		/// <summary>
		/// Parse command line.
		/// </summary>
		public static CmdLineNode ParseCommandLine(string source)
		{
			var input = source.ToCharInput();
			input = input.ConsumeSpaces();
			var programName = ParseQuotedOrNonquotedValue(input);
			var rest = programName.InputRest;

			var cmdOrOpts = rest.ConsumeTillEof(inp => ParseCommandOrOption(inp));
			var cmds = new List<CommandNode>();
			var opts = new List<OptionNode>();
			foreach (var cmdOrOpt in cmdOrOpts.Result)
				if (cmdOrOpt.Command != null)
					cmds.Add(cmdOrOpt.Command);
				else
					opts.Add(cmdOrOpt.Option);
			rest = cmdOrOpts.InputRest;

			rest = rest.ConsumeSpaces();

			if (!rest.IsEof())
				throw new ParsingException("End of file expected", rest.Position);

			return
				new CmdLineNode(
					source,
					0,
					source.Length,
					programName.Result,
					cmds.ToArray(),
					opts.ToArray());
		}

		private static ParseResult<QuotedOrNonquotedValueNode> ParseQuotedValue(ICharInput input)
		{
			var startPos = input.Position;
			input = input.ConsumeChar(_quota);
			var res = input.ConsumeWhile(_quota);
			input = res.InputRest.ConsumeChar(_quota);
			return
				CreateResult(
					new QuotedOrNonquotedValueNode(res.Result, startPos, input.Position - startPos, true),
					input);
		}

		private static ParseResult<QuotedOrNonquotedValueNode> ParseNonquotedValue(ICharInput input)
		{
			var res = input.ConsumeWhileNonSpace();
			return
				CreateResult(
					new QuotedOrNonquotedValueNode(res.Result, input.Position, res.InputRest.Position - input.Position, false),
					res.InputRest);
		}

		private static ParseResult<QuotedOrNonquotedValueNode> ParseQuotedOrNonquotedValue(ICharInput input)
		{
			return input.Current == _quota ? ParseQuotedValue(input) : ParseNonquotedValue(input);
		}

		#region Commands and options
		private static bool IsOptionPrefix(char prefixChar)
		{
			return prefixChar == '/' || prefixChar == '-';
		}

		private static ParseResult<CommandOrOption> ParseCommandOrOption(ICharInput input)
		{
			input = input.ConsumeSpaces();
			if (IsOptionPrefix(input.Current))
			{
				var option = ParseOption(input);
				return new ParseResult<CommandOrOption>(new CommandOrOption(option.Result), option.InputRest);
			}
			var command = ParseCommand(input);
			return
				command == null
					? null
					: new ParseResult<CommandOrOption>(new CommandOrOption(command.Result), command.InputRest);
		}

		private static ParseResult<CommandNode> ParseCommand(ICharInput input)
		{
			var res = input.ConsumeWhileNonSpace();
			if (input.IsEof())
				return null;
			return
				CreateResult(
					new CommandNode(res.Result, input.Position, res.InputRest.Position - input.Position),
					res.InputRest);
		}

		private static ParseResult<OptionNode> ParseOption(ICharInput input)
		{
			var startPos = input.Position;

			if (!IsOptionPrefix(input.Current))
				throw new ParsingException("invalid option prefix '{0}'");
			input = input.GetNext();

			var name =
				input.ConsumeWhile(
					c =>
						c != CharInput.EOF
							&& !char.IsWhiteSpace(c)
							&& c != '='
							&& c != '+'
							&& c != '-');
			if (name.Result.Length == 0)
				throw new ParsingException("option name expected", input.Position);

			var nextChar = name.InputRest.Current;
			if (nextChar == '+' || nextChar == '-')
			{
				return
					CreateResult(
						new OptionNode(name.Result, startPos, name.InputRest.Position - startPos + 1, nextChar == '+'),
						name.InputRest.GetNext());
			}

			if (nextChar == '=')
			{
				var value = ParseQuotedOrNonquotedValue(name.InputRest.GetNext());
				if (value.Result.Text.Length == 0)
					throw new ParsingException(
						"option '{0}' value not specified".FormatStr(name.Result),
						value.Result.Position);
				return
					new ParseResult<OptionNode>(
						new OptionNode(name.Result, startPos, value.InputRest.Position - startPos, value.Result),
						value.InputRest);
			}

			return
				CreateResult(
					new OptionNode(name.Result, startPos, name.InputRest.Position - startPos),
					name.InputRest);
		}
		#endregion

		#region CommandOrOption class
		private class CommandOrOption
		{
			private readonly OptionNode _option;
			private readonly CommandNode _command;

			public CommandOrOption(CommandNode command)
			{
				_command = command;
			}

			public CommandOrOption(OptionNode option)
			{
				_option = option;
			}

			public OptionNode Option
			{
				get { return _option; }
			}

			public CommandNode Command
			{
				get { return _command; }
			}
		}
		#endregion
	}
}
