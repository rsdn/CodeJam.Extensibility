namespace Rsdn.SmartApp
{
	/// <summary>
	/// Стратегия регистрации active parts.
	/// </summary>
	public class ActivePartRegStrategy : RegElementsStrategy<ActivePartInfo, ActivePartAttribute>
	{
		/// <include file='../CommonXmlDocs.xml' path='common-docs/def-ctor/*'/>
		public ActivePartRegStrategy(IServicePublisher publisher) : base(publisher)
		{}

		/// <summary>
		/// Создать описатель.
		/// </summary>
		public override ActivePartInfo CreateElement(ExtensionAttachmentContext context, ActivePartAttribute attr)
		{
			var contract = typeof (IActivePart).AssemblyQualifiedName;
			if (!context.Type.IsImplemented(contract))
				throw new ExtensibilityException(
					"Type '{0}' must implement interface '{1}".FormatStr(context.Type, contract));
			return new ActivePartInfo(context.Type.AssemblyQualifiedName);
		}
	}
}