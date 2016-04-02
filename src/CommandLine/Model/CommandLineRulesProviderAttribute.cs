using System;

using JetBrains.Annotations;

namespace CodeJam.Extensibility.CommandLine
{
	/// <summary>
	/// Mark command line rules provider.
	/// </summary>
	[MeansImplicitUse]
	[BaseTypeRequired(typeof (ICommandLineRulesProvider))]
	public class CommandLineRulesProviderAttribute : Attribute
	{}
}