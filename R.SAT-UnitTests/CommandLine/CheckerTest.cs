using NUnit.Framework;

namespace Rsdn.SmartApp.CommandLine
{
	[TestFixture]
	public class CheckerTest
	{
		[Test]
		public void Test01()
		{
			CommandLineHelper.Check(
				"program cmd1 cmd2 /opt1 /opt2+ /opt3=val3",
				new CmdLineRules(
					new []
					{
						new CommandRule("cmd1"),
						new CommandRule("cmd2"),
					},
					new []
					{
						new OptionRule("opt1", OptionType.Valueless),
						new OptionRule("opt2", OptionType.Bool),
						new OptionRule("opt3", OptionType.Value),
					}));
		}

		[Test]
		[ExpectedException(typeof(CommandLineCheckException), ExpectedMessage = "Unknown commands :'cmd1', 'cmd3'")]
		public void UnknownCommands()
		{
			CommandLineHelper.Check(
				"program cmd1 cmd2 cmd3",
				new CmdLineRules(
					new[]
					{
						new CommandRule("cmd2"),
					}));
		}

		[Test]
		[ExpectedException(typeof(CommandLineCheckException), ExpectedMessage = "Unknown options :'opt2', 'opt3'")]
		public void UnknownOptions()
		{
			CommandLineHelper.Check(
				"program /opt1 /opt2 /opt3",
				new CmdLineRules(
					new[]
					{
						new OptionRule("opt1")
					}));
		}

		[Test]
		[ExpectedException(typeof(CommandLineCheckException), ExpectedMessage = "Option 'opt1' cannot have value")]
		public void OptionsNoValueless()
		{
			CommandLineHelper.Check(
				"program /opt1+",
				new CmdLineRules(
					new[]
					{
						new OptionRule("opt1")
					}));
		}

		[Test]
		[ExpectedException(typeof(CommandLineCheckException), ExpectedMessage = "'+' or '-' must be specified for option 'opt1'")]
		public void OptionsNoBool()
		{
			CommandLineHelper.Check(
				"program /opt1=3",
				new CmdLineRules(
					new[]
					{
						new OptionRule("opt1", OptionType.Bool)
					}));
		}

		[Test]
		[ExpectedException(typeof(CommandLineCheckException), ExpectedMessage = "Value must be specified for option 'opt1'")]
		public void OptionsNoValue()
		{
			CommandLineHelper.Check(
				"program /opt1-",
				new CmdLineRules(
					new[]
					{
						new OptionRule("opt1", OptionType.Value)
					}));
		}

		[Test]
		[ExpectedException(typeof(CommandLineCheckException), ExpectedMessage = "Maximum one command is allowed.")]
		public void ZeroOrOneCommandFail()
		{
			CommandLineHelper.Check(
				"program cmd1 cmd2",
				new CmdLineRules(
					CommandQuantifier.ZeroOrOne,
					new[]
					{
						new CommandRule("cmd1"),
						new CommandRule("cmd2")
					}));
		}

		[Test]
		[ExpectedException(typeof(CommandLineCheckException), ExpectedMessage = "Required command not specified.")]
		public void OneCommandFail1()
		{
			CommandLineHelper.Check(
				"program",
				new CmdLineRules(
					CommandQuantifier.One,
					new[]
					{
						new CommandRule("cmd1"),
						new CommandRule("cmd2")
					}));
		}

		[Test]
		[ExpectedException(typeof(CommandLineCheckException), ExpectedMessage = "Maximum one command is allowed.")]
		public void OneCommandFail2()
		{
			CommandLineHelper.Check(
				"program cmd1 cmd2",
				new CmdLineRules(
					CommandQuantifier.One,
					new[]
					{
						new CommandRule("cmd1"),
						new CommandRule("cmd2")
					}));
		}

		[Test]
		[ExpectedException(typeof(CommandLineCheckException), ExpectedMessage = "Required command(s) not specified.")]
		public void OneOrMultipleCommandFail()
		{
			CommandLineHelper.Check(
				"program",
				new CmdLineRules(
					CommandQuantifier.OneOrMultiple,
					new[]
					{
						new CommandRule("cmd1"),
						new CommandRule("cmd2")
					}));
		}

		[Test]
		[ExpectedException(typeof(CommandLineCheckException), ExpectedMessage = "'cmd1' command(s) must be specified for use opt1 option.")]
		public void NoDepCommand()
		{
			CommandLineHelper.Check(
				"program /opt1+",
				new CmdLineRules(
					new[]
					{
						new OptionRule("opt1", OptionType.Bool, true, "cmd1"),
					}));
		}

		[Test]
		[ExpectedException(typeof(CommandLineCheckException), ExpectedMessage = "Required option 'opt1' absent.")]
		public void GlobalReqOption()
		{
			CommandLineHelper.Check(
				"program",
				new CmdLineRules(
					new[]
					{
						new OptionRule("opt1", OptionType.Bool, true),
					}));
		}

		[Test]
		public void LocalReqOption()
		{
			CommandLineHelper.Check(
				"program",
				new CmdLineRules(
					new[]
					{
						new OptionRule("opt1", OptionType.Bool, true, "cmd1"),
					}));
		}

		[Test]
		[ExpectedException(typeof(CommandLineCheckException), ExpectedMessage = "Required option 'opt1' absent.")]
		public void LocalReqOptionFail()
		{
			CommandLineHelper.Check(
				"program cmd1",
				new CmdLineRules(
					new[]
					{
						new CommandRule("cmd1")
					},
					new[]
					{
						new OptionRule("opt1", OptionType.Bool, true, "cmd1"),
					}));
		}
	}
}