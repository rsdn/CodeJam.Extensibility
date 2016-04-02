using System;

namespace CodeJam.Extensibility.CommandLine
{
	/// <summary>
	/// Information about command line rules provider.
	/// </summary>
	public class CommandLineRulesProviderInfo
	{
		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CommandLineRulesProviderInfo(Type providerType)
		{
			ProviderType = providerType;
		}

		/// <summary>
		/// Provider type.
		/// </summary>
		public Type ProviderType { get; }
	}
}