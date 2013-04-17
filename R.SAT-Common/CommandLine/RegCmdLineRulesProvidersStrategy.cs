namespace Rsdn.SmartApp.CommandLine
{
	/// <summary>
	/// Strategy for registering command line rules provider.
	/// </summary>
	public class RegCmdLineRulesProvidersStrategy
		: RegElementsStrategy<CommandLineRulesProviderInfo, CommandLineRulesProviderAttribute>
	{
		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public RegCmdLineRulesProvidersStrategy(IServicePublisher publisher) : base(publisher)
		{}

		/// <summary>
		/// See base.CreateElement.
		/// </summary>
		public override CommandLineRulesProviderInfo CreateElement(
			ExtensionAttachmentContext context,
			CommandLineRulesProviderAttribute attr)
		{
			var contract = typeof (ICommandLineRulesProvider);
			if (contract.IsAssignableFrom(context.Type))
				throw new ExtensibilityException(
					"Type '{0}' must implement interface '{1}".FormatStr(context.Type, contract));
			return new CommandLineRulesProviderInfo(context.Type);
		}
	}
}