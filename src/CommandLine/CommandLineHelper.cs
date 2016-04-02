using System;
using System.IO;

using CodeJam.Extensibility.CommandLine.Parsing;

using JetBrains.Annotations;

using CommandLineParser = CodeJam.Extensibility.CommandLine.Parsing.CommandLineParser;

namespace CodeJam.Extensibility.CommandLine
{
	/// <summary>
	/// Helper methods for work with command line.
	/// </summary>
	public static class CommandLineHelper
	{
		/// <summary>
		/// Parse command line.
		/// </summary>
		public static CmdLineNode ParseCommandLine(string source)
		{
			return CommandLineParser.ParseCommandLine(source);
		}

		/// <summary>
		/// Check command line semantics.
		/// </summary>
		public static void Check(string commandLine, CmdLineRules rules)
		{
			Check(CommandLineParser.ParseCommandLine(commandLine), rules);
		}

		/// <summary>
		/// Check command line semantics.
		/// </summary>
		public static void Check(CmdLineNode commandLine, CmdLineRules rules)
		{
			CommandLineChecker.Check(commandLine, rules);
		}

		/// <summary>
		/// Print usage.
		/// </summary>
		public static void PrintUsage(
			[NotNull] this CmdLineRules rules,
			[NotNull] TextWriter writer,
			[NotNull] PrintUsageSettings settings)
		{
			if (rules == null) throw new ArgumentNullException(nameof(rules));
			if (writer == null) throw new ArgumentNullException(nameof(writer));
			if (settings == null) throw new ArgumentNullException(nameof(settings));

			UsagePrinter.PrintUsage(rules, writer, settings);
		}

		/// <summary>
		/// Print usage.
		/// </summary>
		public static string PrintUsage(
			[NotNull] this CmdLineRules rules,
			[NotNull] PrintUsageSettings settings)
		{
			var sw = new StringWriter();
			PrintUsage(rules, sw, settings);
			return sw.ToString();
		}

		/// <summary>
		/// Print usage with default settings.
		/// </summary>
		public static void PrintUsage(
			[NotNull] this CmdLineRules rules,
			[NotNull] TextWriter writer)
		{
			PrintUsage(rules, writer, new PrintUsageSettings());
		}
	}
}