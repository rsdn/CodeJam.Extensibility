using System;

namespace Rsdn.SmartApp.CommandLine
{
	/// <summary>
	/// Information about command line rules provider.
	/// </summary>
	public class CommandLineRulesProviderInfo
	{
		private readonly Type _providerType;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public CommandLineRulesProviderInfo(Type providerType)
		{
			_providerType = providerType;
		}

		/// <summary>
		/// Provider type.
		/// </summary>
		public Type ProviderType
		{
			get { return _providerType; }
		}
	}
}