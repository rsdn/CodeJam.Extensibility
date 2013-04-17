using System;

using JetBrains.Annotations;

namespace Rsdn.SmartApp.CommandLine
{
	///<summary>
	/// Root of the command line AST.
	///</summary>
	public class CmdLineNode : CmdLineNodeBase
	{
		private readonly QuotedOrNonquotedValueNode _programName;
		private readonly CommandNode[] _commands;
		private readonly OptionNode[] _options;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CmdLineNode(
			string text,
			int position,
			int length,
			[NotNull] QuotedOrNonquotedValueNode programName,
			[NotNull] CommandNode[] commands,
			[NotNull] OptionNode[] options) : base(text, position, length)
		{
			if (programName == null)
				throw new ArgumentNullException("programName");
			if (commands == null)
				throw new ArgumentNullException("commands");
			if (options == null)
				throw new ArgumentNullException("options");
			_programName = programName;
			_commands = commands;
			_options = options;
		}

		///<summary>
		/// Program name node.
		///</summary>
		[NotNull]
		public QuotedOrNonquotedValueNode ProgramName
		{
			get { return _programName; }
		}

		/// <summary>
		/// Commands.
		/// </summary>
		public CommandNode[] Commands
		{
			get { return _commands; }
		}

		/// <summary>
		/// Options.
		/// </summary>
		public OptionNode[] Options
		{
			get { return _options; }
		}
	}
}